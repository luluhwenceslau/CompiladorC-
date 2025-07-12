using System;
using LLVMSharp.Interop;

namespace Compilador
{
    public class GeradorLLVM
    {
        public LLVMContextRef Context;
        public LLVMModuleRef Module;
        public LLVMBuilderRef Builder;

        public GeradorLLVM(string nomeModulo = "meu_modulo")
        {
            Context = LLVMContextRef.Create();
            Module = Context.CreateModuleWithName(nomeModulo);
            Builder = Context.CreateBuilder();
        }

        // Cria um tipo inteiro de 32 bits
        public LLVMTypeRef Int32Type => LLVMTypeRef.Int32;

        // Cria um valor constante inteiro
        public LLVMValueRef ConstInt32(int valor)
        {
            return LLVMValueRef.CreateConstInt(Int32Type, (ulong)valor, false);
        }

        // Exemplo simples: gera código para soma de dois inteiros constantes
        public LLVMValueRef GerarSoma(LLVMValueRef lhs, LLVMValueRef rhs)
        {
            return Builder.BuildAdd(lhs, rhs, "soma_tmp");
        }

        // Método para criar uma função simples (exemplo)
        public LLVMValueRef CriarFuncaoSoma()
        {
            // Define o tipo da função: int soma(int a, int b)
            var paramTypes = new LLVMTypeRef[] { Int32Type, Int32Type };
            var funcType = LLVMTypeRef.CreateFunction(Int32Type, paramTypes, false);

            var function = Module.AddFunction("soma", funcType);

            var entry = function.AppendBasicBlock("entry");
            Builder.PositionAtEnd(entry);

            var a = function.Params[0];
            var b = function.Params[1];

            var soma = Builder.BuildAdd(a, b, "resultado");

            Builder.BuildRet(soma);

            return function;
        }

        // Salva o módulo LLVM IR em arquivo
        public void SalvarIR(string caminho)
        {
            if (!Module.TryPrintToFile(caminho, out string errorMessage))
            {
                Console.WriteLine($"Erro ao salvar IR: {errorMessage}");
            }
        }

        // Exibe o LLVM IR no console
        public void ExibirIR()
        {
            Module.Dump();
        }
    }
}
