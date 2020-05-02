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
    public class HttpClientLibGenerator : ICodeGenerator
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
        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context,
            IProgress<Diagnostic> progress,
            CancellationToken cancellationToken)
        {
            var applyToInterface = (InterfaceDeclarationSyntax)context.ProcessingNode;

            var classDeclaration = SyntaxFactory.ClassDeclaration(SyntaxFactory
                .Identifier(applyToInterface.Identifier.ValueText.Remove(0,1)));

            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var interfaceMembers = applyToInterface.Members;

            foreach(var member in interfaceMembers)
            {
                classDeclaration = AddImplementation(member, classDeclaration);
            }

            SyntaxList<MemberDeclarationSyntax> results = SyntaxFactory
               .SingletonList<MemberDeclarationSyntax>(classDeclaration);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Adds the implementation.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="classDeclaration">The class declaration.</param>
        private ClassDeclarationSyntax AddImplementation(MemberDeclarationSyntax memberDeclarationSyntax, 
            ClassDeclarationSyntax classDeclaration)
        {
            
            var methodBody = SyntaxFactory.ParseStatement("return null;");

            var methodSignature = ((MethodDeclarationSyntax)memberDeclarationSyntax);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                methodSignature.ReturnType,
                methodSignature.Identifier.ValueText)
               .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
               .WithBody(SyntaxFactory.Block(methodBody));

            // Add the field, the property and method to the class.
            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            return classDeclaration;
        }
    }
}
