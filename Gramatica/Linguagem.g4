grammar Linguagem;

programa: comando+ ;

comando
    : escreva
    | leia
    | atribuicao
    | declaracao
    | se
    | enquanto
    ;

escreva: 'escreva' '(' expressao ')' ';' ;

leia: 'leia' '(' ID ')' ';' ;

declaracao: tipo ID ('=' expressao)? ';' ;

atribuicao: ID '=' expressao ';' ;

se: 'se' '(' expressao ')' bloco ;

enquanto: 'enquanto' '(' expressao ')' bloco ;

bloco: '{' comando+ '}' ;

expressao
    : expressao op=('+' | '-' | '*' | '/') expressao  #operacao
    | expressao opRel=('>' | '<' | '==' | '!=') expressao #comparacao
    | INT                                                #inteiro
    | TEXTO                                              #texto
    | ID                                                 #variavel
    ;

tipo: 'inteiro' | 'texto' ;

TEXTO: '"' (~["\r\n])* '"' ;
ID: [a-zA-Z_][a-zA-Z0-9_]* ;
INT: [0-9]+ ;

WS: [ \t\r\n]+ -> skip ;
