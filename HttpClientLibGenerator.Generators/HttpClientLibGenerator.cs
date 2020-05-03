using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HttpClientLibGenerator.Generators
{
    public class HttpClientLibGenerator : IRichCodeGenerator
    {
        private readonly string suffix;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientLibGenerator"/> class.
        /// </summary>
        /// <param name="attributeData">The attribute data.</param>
        public HttpClientLibGenerator(AttributeData attributeData)
        {
            suffix = (string)attributeData.ConstructorArguments[0].Value;

            //System.Diagnostics.Debugger.Launch();

            //while (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    Thread.Sleep(500);
            //}
        }

        /// <summary>
        /// Create the syntax tree representing the expansion of some member to which this attribute is applied.
        /// </summary>
        /// <param name="context">All the inputs necessary to perform the code generation.</param>
        /// <param name="progress">A way to report diagnostic messages.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// The generated member syntax to be added to the project.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context,
            IProgress<Diagnostic> progress,
            CancellationToken cancellationToken)
        {

            var syntaxFactory = SyntaxFactory.CompilationUnit();

            syntaxFactory = AddUsings(syntaxFactory);

            var applyToInterface = (InterfaceDeclarationSyntax)context.ProcessingNode;

            var namespaceDeclaration = SyntaxFactory
                .NamespaceDeclaration(SyntaxFactory.ParseName("CodeGenerator")).NormalizeWhitespace();
            
            ClassDeclarationSyntax classDeclaration = GetClassDeclaration(applyToInterface);

            var interfaceMembers = applyToInterface.Members;

            foreach (var member in interfaceMembers)
            {
                classDeclaration = AddImplementation(member, classDeclaration);
            }

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            syntaxFactory = syntaxFactory.AddMembers(namespaceDeclaration);

            var richGenerationResult = new RichGenerationResult();

            richGenerationResult.Members = syntaxFactory.Members;

            richGenerationResult.Usings = syntaxFactory.Usings;

            return Task.FromResult(richGenerationResult);
        }

        /// <summary>
        /// Gets the class declaration.
        /// </summary>
        /// <param name="applyToInterface">The apply to interface.</param>
        /// <returns></returns>
        private static ClassDeclarationSyntax GetClassDeclaration(InterfaceDeclarationSyntax applyToInterface)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(SyntaxFactory
                            .Identifier(applyToInterface.Identifier.ValueText.Remove(0, 1)));

            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Implementing the interface
            classDeclaration = classDeclaration.AddBaseListTypes(SyntaxFactory.
                SimpleBaseType(SyntaxFactory.ParseTypeName(applyToInterface.Identifier.ValueText)));
            
            FieldDeclarationSyntax httpClient = AddVariable("HttpClient", "httpClient");
            FieldDeclarationSyntax remoteServiceBaseUrl = AddVariable("string", "remoteServiceBaseUrl");

            classDeclaration = classDeclaration.AddMembers(httpClient, remoteServiceBaseUrl);

            return classDeclaration;
        }

        /// <summary>
        /// Adds the variable.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static FieldDeclarationSyntax AddVariable(string typeName, string fieldName)
        {
            var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(typeName))
                            .AddVariables(SyntaxFactory.VariableDeclarator(fieldName));

            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            return fieldDeclaration;
        }

        /// <summary>
        /// Adds the usings.
        /// </summary>
        /// <param name="syntaxFactory">The syntax factory.</param>
        /// <returns></returns>
        private CompilationUnitSyntax AddUsings(CompilationUnitSyntax syntaxFactory)
        {
            syntaxFactory = syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Net.Http")));
            syntaxFactory = syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Newtonsoft.Json")));
            return syntaxFactory;
        }

        /// <summary>
        /// Adds the implementation.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="classDeclaration">The class declaration.</param>
        private ClassDeclarationSyntax AddImplementation(MemberDeclarationSyntax memberDeclarationSyntax,
            ClassDeclarationSyntax classDeclaration)
        {

            //string methodBody = "return null;";

            string methodBody = @"var responseString = await httpClient.GetStringAsync(remoteServiceBaseUrl);
                                  var users = JsonConvert.DeserializeObject<string>(responseString);
                                  return users;";

            var parsedBody = SyntaxFactory.ParseStatement(methodBody);

            var methodSignature = ((MethodDeclarationSyntax)memberDeclarationSyntax);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                methodSignature.ReturnType,
                methodSignature.Identifier.ValueText)
               .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
               .WithBody(SyntaxFactory.Block(parsedBody.NormalizeWhitespace()));

            // Add the field, the property and method to the class.
            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            return classDeclaration;
        }

        /// <summary>
        /// Create the syntax tree representing the expansion of some member to which this attribute is applied.
        /// </summary>
        /// <param name="context">All the inputs necessary to perform the code generation.</param>
        /// <param name="progress">A way to report diagnostic messages.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// The generated member syntax to be added to the project.
        /// </returns>
        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
