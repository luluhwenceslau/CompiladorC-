grammar Linguagem;

programa: comando+ ;

comando: escreva ;

escreva: 'escreva' '(' TEXTO ')' ';' ;

TEXTO: '"' (~["\r\n])* '"' ;

WS: [ \t\r\n]+ -> skip ;
