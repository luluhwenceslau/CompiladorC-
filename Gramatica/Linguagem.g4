grammar Linguagem;

programa: comando+ ;

comando
    : escreva
    | leia
    | atribuicao
    | declaracao
    | se
    | enquanto
    | retorne
    ;

escreva: 'escreva' '(' expressao ')' ';' ;
leia: 'leia' '(' ID ')' ';' ;
retorne: 'retorne' expressao? ';' ;

declaracao: tipo ID ('=' expressao)? ';' ;
atribuicao: ID '=' expressao ';' ;

se: 'se' '(' expressao ')' bloco ;
enquanto: 'enquanto' '(' expressao ')' bloco ;

bloco: '{' comando+ '}' ;

expressao
    : expressao op=('+' | '-' | '*' | '/') expressao     #operacao
    | expressao opRel=('>' | '<' | '==' | '!=') expressao #comparacao
    | '(' expressao ')'                                   #agrupamento
    | INT                                                 #inteiro
    | FLOAT                                               #decimal
    | BOOLEANO                                            #booleano
    | TEXTO                                               #texto
    | ID                                                  #variavel
    ;

tipo: 'inteiro' | 'texto' | 'booleano' ;

TEXTO: '"' (~["\r\n])* '"' ;
BOOLEANO: 'verdadeiro' | 'falso' ;
ID: [a-zA-Z_][a-zA-Z0-9_]* ;
INT: [0-9]+ ;
FLOAT: [0-9]+ '.' [0-9]+ ;

COMENTARIO: '//' ~[\r\n]* -> skip ;
WS: [ \t\r\n]+ -> skip ;
