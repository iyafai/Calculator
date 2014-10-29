//Imran Yafai
//Comp Physics - Project 7

#include <iostream>
#include <stdio.h>
#include <math.h>

#define PI atan(1)*4

using namespace std;

double integral(int order, int Xpow, double start, double end)
{
	double valueS=0, valueE=0;
	if(order=0)
	{
		if(Xpow=1)
		{
			valueS = start*exp(-1*start*start);
			valueE = end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = start*start*exp(-1*start*start);
			valueE = end*end*exp(-1*end*end);
		}
	}
	else if(order==1)
	{
		if(Xpow=1)
		{
			valueS = 2*start*start*start*exp(-1*start*start);
			valueE = 2*end*end*end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = 2*start*start*start*start*exp(-1*start*start);
			valueE = 2*end*end*end*end*exp(-1*end*end);
		}
	}
	else if(order=2)
	{
		if(Xpow=1)
		{
			valueS = .5*(2*start*start-1)*(2*start*start-1)*start*exp(-1*start*start);
			valueE = .5*(2*end*end-1)*(2*end*end-1)*end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = .5*(2*start*start-1)*(2*start*start-1)*start*start*exp(-1*start*start);
			valueE = .5*(2*end*end-1)*(2*end*end-1)*end*end*exp(-1*end*end);
		}
	}
	double sum = 0;
	
	if((valueE-valueS) < .001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral(order, Xpow, start, (end+start)/2) + integral(order, Xpow, (end+start)/2, end);
	return sum;
}

double integral2(int order, int Xpow, double start, double end)
{
	double valueS=0, valueE=0;
	if(order=0)
	{
		if(Xpow=1)
		{
			valueS = start*exp(-1*start*start);
			valueE = end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = start*start*exp(-1*start*start);
			valueE = end*end*exp(-1*end*end);
		}
	}
	else if(order==1)
	{
		if(Xpow=1)
		{
			valueS = 2*start*start*start*exp(-1*start*start);
			valueE = 2*end*end*end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = 2*start*start*start*start*exp(-1*start*start);
			valueE = 2*end*end*end*end*exp(-1*end*end);
		}
	}
	else if(order=2)
	{
		if(Xpow=1)
		{
			valueS = .5*(2*start*start-1)*(2*start*start-1)*start*exp(-1*start*start);
			valueE = .5*(2*end*end-1)*(2*end*end-1)*end*exp(-1*end*end);
		}
		else if(Xpow=2)
		{
			valueS = .5*(2*start*start-1)*(2*start*start-1)*start*start*exp(-1*start*start);
			valueE = .5*(2*end*end-1)*(2*end*end-1)*end*end*exp(-1*end*end);
		}
	}
	double sum = 0;
	
	if((valueS-valueE) < .00001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral2(order, Xpow, start, (end+start)/2) + integral2(order, Xpow, (end+start)/2, end);
	return sum;
}

double test(double start, double end)
{
	double valueS=start*sin(start)*sin(start), valueE=end*sin(end)*sin(end);
	
	double sum = 0;
	
	if((valueS-valueE) < .0001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = test(start, (end+start)/2) + test((end+start)/2, end);
	return sum;
}

double test2(double start, double end)
{
	double valueS=start*sin(start)*sin(start), valueE=end*sin(end)*sin(end);
	
	double sum = 0;
	
	if((valueE-valueS) < .0001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = test(start, (end+start)/2) + test((end+start)/2, end);
	return sum;
}

int main()
{
	cout<<"<X> n=0 = "<<integral(0,1,-999999,0)+integral2(0,1,0,999999)<<endl;
	cout<<"<X> n=1 = "<<integral(1,1,-999999,0)+integral(1,1,0,999999)<<endl;
	cout<<"<X> n=2 = "<<integral(2,1,-999999,0)+integral(2,1,0,999999)<<endl;
	cout<<"<X^2> n=0 = "<<integral(0,2,-999999,0)+integral(0,2,0,999999)<<endl;
	cout<<"<X^2> n=1 = "<<integral(1,2,-999999,0)+integral(1,2,0,999999)<<endl;
	cout<<"<X^2> n=2 = "<<integral(2,2,-999999,0)+integral(2,2,0,999999)<<endl;
	cout<<"test <X> of sin(x) = "<<test(0,PI/2)+test2(PI/2,PI)<<endl;
}