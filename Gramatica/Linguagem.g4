grammar Linguagem;

// Regras da gramática (Parser)

programa: (comando | declaracao | funcao | classe)* EOF;

declaracao
    : tipo ID (ABRE_COLCH INT FECHA_COLCH)? (IGUAL expressao)? PONTO_VIRGULA
    ;

comando
    : atribuicao
    | leitura
    | escrita
    | decisao
    | repeticao
    | chamadaFuncao
    | retorno
    ;

atribuicao
    : (ID | ID ABRE_COLCH INT FECHA_COLCH) IGUAL expressao PONTO_VIRGULA
    ;

leitura
    : LEIA ABRE_PAREN ID FECHA_PAREN PONTO_VIRGULA
    ;

escrita
    : ESCREVA ABRE_PAREN expressao FECHA_PAREN PONTO_VIRGULA
    ;

decisao
    : SE ABRE_PAREN expressao FECHA_PAREN bloco
    ;

repeticao
    : ENQUANTO ABRE_PAREN expressao FECHA_PAREN bloco
    ;

retorno
    : RETURN expressao PONTO_VIRGULA
    ;

funcao
    : tipo ID ABRE_PAREN parametros? FECHA_PAREN bloco
    ;

parametros
    : parametro (VIRGULA parametro)*
    ;

parametro
    : tipo ID
    ;

chamadaFuncao
    : ID ABRE_PAREN (expressao (VIRGULA expressao)*)? FECHA_PAREN PONTO_VIRGULA
    ;

classe
    : CLASSE ID ABRE_CHAVE (declaracao | funcao)* FECHA_CHAVE
    ;

bloco
    : ABRE_CHAVE (comando | declaracao)* FECHA_CHAVE
    ;

expressao
    : expressao op=(MAIS | MENOS | MULT | DIV) expressao
    | expressao op=(MAIOR | MENOR | MAIOR_IGUAL | MENOR_IGUAL | IGUAL_IGUAL | DIFERENTE) expressao
    | ABRE_PAREN expressao FECHA_PAREN
    | ID
    | ID ABRE_COLCH INT FECHA_COLCH
    | INT
    | FLOAT_LITERAL
    | CHAR_LITERAL
    | STRING_LITERAL
    ;

// ✅ Regra adicionada para tipos
tipo
    : INTEIRO
    | FLOAT
    | CHAR
    | TEXTO
    ;

// Regras do Léxico (Lexer)

INTEIRO     : 'inteiro';
FLOAT       : 'float';
CHAR        : 'char';
TEXTO       : 'texto';
RETURN      : 'return';
SE          : 'se';
ENQUANTO    : 'enquanto';
LEIA        : 'leia';
ESCREVA     : 'escreva';
CLASSE      : 'classe';

MAIS        : '+';
MENOS       : '-';
MULT        : '*';
DIV         : '/';
IGUAL       : '=';
MAIOR       : '>';
MENOR       : '<';
MAIOR_IGUAL : '>=';
MENOR_IGUAL : '<=';
IGUAL_IGUAL : '==';
DIFERENTE   : '!=';

ABRE_PAREN  : '(';
FECHA_PAREN : ')';
ABRE_CHAVE  : '{';
FECHA_CHAVE : '}';
ABRE_COLCH  : '[';
FECHA_COLCH : ']';

PONTO_VIRGULA : ';';
VIRGULA       : ',';

ID            : [a-zA-Z_][a-zA-Z_0-9]*;
INT           : [0-9]+;
FLOAT_LITERAL : [0-9]+ '.' [0-9]+;
CHAR_LITERAL  : '\'' . '\'';
STRING_LITERAL: '"' (~["\\] | '\\' .)* '"';

ESPACO        : [ \t\r\n]+ -> skip;
COMENTARIO    : '//' ~[\r\n]* -> skip;
