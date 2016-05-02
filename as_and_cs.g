namespaceDeclaration := ts?, namespace, (ts, identifier)?, ts?

ts := (whitespace+ / comment_area)+

comment_area := comment_line / comment_block
comment_block := comment_start, comment_middle*, comment_end
comment_line := comment_line_start, comment_line_content*, EOL
comment_middle := -comment_end
comment_line_content := -EOL
comment_line_start := "//"
comment_start := "/*"
comment_end := "*/"

importDefinitionPlace := importDefinition*

importDefinition := ts?, import, ts, identifier, SEMI, EOL?

classDefinitionPlace := classDefinition?, ts?
classDefinition := ts?, classModifier*, CLASS, ts, identifier, classBaseClause?, 
    ts?, LCURLY, memberExpression*, ts?, RCURLY
classModifier := scope / FINAL, ts
classBase := 
    (classExtends, interfaceTypeListFollows) / interfaceTypeList / 
    classExtends
classIdentifier := identifier
interfaceTypeList := interfaceFirst, interfaceNextPlace
interfaceNextPlace := (ts?, COMMA, ts?, interfaceIdentifier)*
interfacePrefix := I, uppercasechar, identifier?
interfaceIdentifier := interfacePrefix, identifier?

namespaceModifierPlace := namespaceModifiers?
namespaceModifiers := (scope / STATIC / FINAL / OVERRIDE, ts)+
functionBody := ts?, LCURLY, statement*, ts?, RCURLY
functionDeclaration := functionModified / functionDefault
functionSignature := FUNCTION, ts, identifier, functionParameters
functionDefinition := functionDeclaration, functionBody
functionParameters := ts?, LPAREN, ts?, argumentList?, ts?, RPAREN

argumentList := argumentDeclaration, (ts?, COMMA, ts?, argumentDeclaration)*
argumentDeclaration := argumentInitialized / argumentDeclared
argumentInitializer := ts?, ASSIGN, ts?, assignmentValue
# assignmentValue := -argumentEnd+
assignmentValue := expression
argumentEnd := SEMI / COMMA / EOL / EOF / RPAREN
variableAssignment := identifier, ts?, ASSIGN, ts?, assignmentValue

memberExpression := functionDefinition / memberDeclaration
memberDeclaration := ts, namespaceModifiers?, variableDeclaration, ts?, SEMI

statement := ts?, primaryExpression, ts?, SEMI
primaryExpression := expression
expression := variableDeclaration
    / variableAssignment
    / callExpression
    / identifier
    / numberFormat
    / string

numberFormat := hex / floatFormat / int

address := identifier, (ts?, PERIOD, ts?, identifier)?
callExpression := address, ts?, LPAREN, ts?, callParameters?, ts?, RPAREN
callParameters := expression, (ts?, COMMA, expression)*

scope := public / internal / protected / private

FUNCTION := "function"

CLASS := "class"

public := "public"
internal := "internal"
protected := "protected"
private := "private"
VOID := "void"

STATIC := "static"

identifier := alphaunder, (PERIOD?, alphanums+)*
alphanums      := (letter / digit)+
alphaunder     := (letter / UNDERSCORE)
dataType := INTEGER / STRING / BOOLEAN / FLOAT / OBJECT
returnType := dataType / VOID

PERIOD := "."
LCURLY := "{"
RCURLY := "}"
LPAREN := "("
RPAREN := ")"
SEMI := ";"
COLON := ":"
COMMA := ","
EOL   := ("\r"?, "\n") / EOF
SPACE := " "
ASSIGN := "="
UNDERSCORE := "_"
QUOTE := "\""
APOS := "'"
OVERRIDE := "override"
I := "I"