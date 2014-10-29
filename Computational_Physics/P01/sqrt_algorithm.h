//Imran Yafai
//Computational Physics Project 1
//Purpose: Implementing algorithims for finding the square root of a number

#include <stdio.h>
#include <iostream>

using namespace std;

double sqrt_brute(double input1)
{
	double attempt=0;
	for(attempt=0; attempt*attempt<=input1; attempt+=.01)	{}; //increments attempt by .01 and checks if squaring it yields correct value
	return attempt;	
}

double sqrt_guess (double input2)
{
	double attempt=(input2/2), largest=input2, smallest=0;
	bool solution=false;
	
	do	/*loop to find square root, takes initial guess and modifies it based on if it squares to a larger or smaller value that the input. */
	{
	    if(attempt*attempt<=(input2+.01) && attempt*attempt>=(input2-.01))
		solution=true; /* if attempt^2 is within .01 of input, the condition for ending the loop is satisfied */
		
	    if(attempt*attempt>input2)
	    {
		largest=attempt; //changes the smallest value that is larger than the input for purposes of modifying the attempt
		attempt=((attempt+smallest)/2); //changes attempt to an average of the current attempt and the largest value that is smaller than the input
	    }
	    else if(attempt*attempt<input2)
	    {
		    smallest=attempt;	//changes the largest value that is smaller than the input, for purposes of modifying the attempt
	  	    attempt=((attempt+largest)/2);	//changes attempt to an average of the current attempt and the smallest value that is larger than the input
	    }
	}while (solution==false);	//continues until this is true (see first if statement)

	return attempt;
}

double sqrt_newton (double input3)
{
	double attempt=(input3/2);
	bool solution2=false;
	do	//runs this loop until the square of the attempt is within .01 of the input
	{
		if(attempt*attempt<(input3+.01) && attempt*attempt>(input3-.01)) //statement for checking said condition
			solution2=true;
		else	
			attempt = attempt - (attempt*attempt-input3)/(2*attempt);	//if the above is unsatisfied will run netwon's method again to modify attempt value.
		
	} while (solution2 == false);
	
	return attempt;
}