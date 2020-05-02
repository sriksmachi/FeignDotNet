using System;
using System.Threading;
using System.Threading.Tasks;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace HttpClientLibGenerator.Generators
{
    public class HttpClientLibGenerator : IRichCodeGenerator
    {
        private readonly string suffix;

        public HttpClientLibGenerator(AttributeData attributeData) 
        {
            suffix = (string)attributeData.ConstructorArguments[0].Value;
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); 
        }

        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            // Our generator is applied to any interface that our attribute is applied to.
            var applyToInterface = (InterfaceDeclarationSyntax)context.ProcessingNode;

            // Apply a suffix to the name of a copy of the class.
            var copy = applyToInterface.WithIdentifier(SyntaxFactory.Identifier(applyToInterface.Identifier.ValueText.Remove(0, 1)));

            // Return our modified copy. It will be added to the user's project for compilation.
            var results = SyntaxFactory.SingletonList<ClassDeclarationSyntax>(copy);

            return Task.FromResult(results);
        }
    }
}
