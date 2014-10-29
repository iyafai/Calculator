//Imran Yafai

#include <stdio.h>
#include <iostream>

using namespace std;

double* LR_integrate(int step, double* x, double* t)
{
	double* temp = new double[step];
	temp[0]=0;

	for(int m=1; m<step; m++)
		temp[m]=(t[m+1]-t[m])*x[m]+temp[m-1];

	return temp;
}

double* MP_integrate(int step, double* x, double* t)
{
	double* temp = new double[step];
	temp[0]=0;

	for(int i=1, k=1; i<(2*step); i+=2, k++)
		temp[k]=(t[i+1]-t[i-1])*x[i]+temp[k-1];

	return temp;
}

