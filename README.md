##Equation Parsing Calculator

A calculator designed to take as input simple equations as text and output results.

###Usage

Command-line entry:

    Calculator.exe Test1.inp Test2.inp ...
An unspecified number of input files can sent at once.  
Alternatively running the program without input opens an interactive mode which allows the solving of equations one by one.  

    Calculator.exe
    Input Equation to solve
    Equations must be of form: variable = Expression;
    eg. x3 = 4*(x1-5);
    type 'quit' to end
    >

The calculator handles the following operations:  
Addition, Subtraction, Multiplication, Division, Integer Division (use div(num,num)), Modulo (use mod(num, num)), Exponentiation, and Negation. Parentheses are also valid.

Equations must be of the form:

variable = expression  
where variable is defined as any letter followed by any number of alphanumeric characters. (e.g. x1, xasdf, r45)

Invalid expressions are noted in output, variables that would be assigned an invalid expression are set to zero, as are values that are undefined or too large.

Output is in the form of "Input Filename".out and will be created in the users' current working directory in a folder called Output.
