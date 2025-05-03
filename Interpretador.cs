using System;

public class Interpretador : LinguagemBaseListener
{
    // Método sobrescrito para o comando "escreva"
    public override void EnterEscreva(LinguagemParser.EscrevaContext context)
    {
        // Pega o texto dentro do comando escreva
        var texto = context.TEXTO().GetText();
        Console.WriteLine(texto); // Executa o comando "escreva"
    }
    
    // Podemos adicionar outros métodos para lidar com diferentes comandos
    // Por exemplo, para um futuro comando "leia" ou outro comando de controle
}
