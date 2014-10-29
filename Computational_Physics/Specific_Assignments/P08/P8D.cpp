#include <iostream>
#include <stdio.h>
#include "Matrix2.h"

using namespace std;
/*
void print(int n)
{
	n++;
	if(n%1000==0)
		cout<<n<<endl;
	if(n<=1000)
	print(n);
}*/

int main()
{
//	print(1);

	Matrix A(3,3);
	Matrix D(10,10);
	int r=1;
	for(int i=0; i<10; i++)
		for(int j=0; j<10; j++)
		{
			D.setEle(i,j,r);
			r++;
		}
	r=1;
	for(int i=0; i<3; i++)
		for(int j=0; j<3; j++)
		{
			A.setEle(i,j,r);
			r++;
		}
	A.Print();
	D.Print();

	cout<<A.Determinant()<<endl;
	cout<<D.Determinant()<<endl;
}