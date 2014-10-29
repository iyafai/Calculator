//Imran Yafai
//Computational Physics Project 10

#include <iostream>
#include <stdio.h>
//#include "Matrix.h"

using namespace std;

double* LaplaceEq(double* data, int points)
{
	double* Dguess = new double[points];
	Dguess = data;
	int iter = 0;
	
	while(iter<20000)
	{
		for(int i=1; i<(points-1); i++)
			Dguess[i] = (Dguess[i-1]+Dguess[i+1])/2;
		iter++;
	}

	return Dguess;

}

int main()
{
	int points = 50;
	double* D = new double[50];
	double* L = new double[50];
	for(int i=1; i<(points-1); i++)
		D[i]=0;
	D[0] = 10;
	D[49] = 0;

	L = LaplaceEq(D, points);
	
	for(int k=0; k<points; k++)
		cout<<L[k]<<endl;


/*	Matrix A(3,3);

	A.setEle(0,0,1);
	A.setEle(1,1,1);
	A.setEle(2,2,1);
	A.setEle(0,1,2);
	A.setEle(0,2,5);
	A.setEle(1,0,10);
	A.setEle(1,2,4);
	A.setEle(2,0,3);
	A.setEle(2,1,3);

	A.Print();
	Matrix AI(3,3);
	AI = A.Invert();

	AI.Print();*/
	
}