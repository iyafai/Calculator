//Imran Yafai
//Comp Phys - Project 12

#include <iostream>
#include <stdio.h>
#include <math.h>

#define me 9.11E-31
#define PI (atan(1)*4)
#define hbar 6.626059E-34
//#define freq sqrt(1.84566188E-11/me)	//#define freq 4501234019.69	//#define freq 90021.6313192
#define freq 1
#define alpha (me*freq/hbar)

using namespace std;

double ExpValue_Hn(int order, int powX, double start, double end, double steps)
{
	double sum=0, stepsize = (end-start)/steps;

	if(order==0)
	{
		if(powX == 1)
			for(double i=start; i<end; i+=stepsize)
				sum+=i*exp(-1*alpha*i*i)*stepsize;
		if(powX == 2)
			for(double i=start; i<end; i+=stepsize)
				sum+=i*i*exp(-1*alpha*i*i)*stepsize;
	}

	if(order==1)
	{
		if(powX == 1)
			for(double i=start; i<end; i+=stepsize)
				sum+=2*alpha*i*i*i*exp(-1*alpha*i*i)*stepsize;
		if(powX == 2)
			for(double i=start; i<end; i+=stepsize)
				sum+=2*alpha*i*i*i*i*exp(-1*alpha*i*i)*stepsize;
	}

	if(order==2)
	{
		if(powX == 1)
			for(double i=start; i<end; i+=stepsize)
				sum+=(2*alpha*i*i-1)/sqrt(2.0)*i*exp(-1*alpha*i*i)*stepsize;
		else if(powX == 2)
			for(double i=start; i<end; i+=stepsize)
				sum+=(2*alpha*i*i-1)/sqrt(2.0)*i*i*exp(-1*alpha*i*i)*stepsize;
	}
	sum=sum*pow((alpha/PI),.5);
	return sum;
}

int main()
{
	cout<<"<X> for n=0: "<<ExpValue_Hn(0,1,-999999,999999,100000000)<<endl;
	cout<<"<X^2> for n=0: "<<ExpValue_Hn(0,2,-999999,999999,100000000)<<endl;
	cout<<"<X> for n=1: "<<ExpValue_Hn(1,1,-999999,999999,100000000)<<endl;
	cout<<"<X^2> for n=1: "<<ExpValue_Hn(1,2,-999999,999999,100000000)<<endl;
	cout<<"<X> for n=2: "<<ExpValue_Hn(2,1,-999999,999999,100000000)<<endl;
	cout<<"<X^2> for n=2: "<<ExpValue_Hn(2,2,-999999,999999,100000000)<<endl;
}