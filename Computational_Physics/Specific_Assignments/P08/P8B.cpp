//Imran Yafai
//Comp Phys Project 8B

#include "Matrix.h"
#include <math.h>

int main()
{
//	cout<<"v = "<<endl;
	Matrix Velocity(100,1);
	for(double i=0, j=0; i<1; i+=.01, j++)
		Velocity.setEle(j,0, (2*sin(3*i)+2));
//	Velocity.Print();
	Matrix Derivative(100,100);
	Derivative.setEle(0,0,-1);
	Derivative.setEle(0,1,1);
	for(int k=1; k<98; k++)
	{
		Derivative.setEle(k,k,1);
		Derivative.setEle(k,k-1,-1);
	}
	Derivative.setEle(98,98,-1);
	Derivative.setEle(98,99,1);
	Derivative.setEle(99,98,-1);
	Derivative.setEle(99,99,1);
//	Derivative.Print();

	Matrix Acceleration(100,1);
	Acceleration = Derivative.Mult(Velocity);
//	cout<<"a = "<<endl;
	Acceleration.Print();
}