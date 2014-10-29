#include <iostream>
#include <stdio.h>

using namespace std;

class Matrix
{
	private:
		double** element;
		int row;
		int col;
	public:
		Matrix(int, int);
		~Matrix();
		void setEle(int, int, double);
		double getEle(int, int);
		void Print();
		Matrix Reduce(int, int);
		double Determinant();
};

Matrix::Matrix(int N, int M)
{
	element = new double*[N];
	row = N;
	col = M;
	for(int i=0; i<N; i++)
	{
		element[i] = new double[M];
		for(int j=0; j<M; j++)
			element[i][j]=0;
	}
			
}

Matrix::~Matrix()	{}

void Matrix::setEle(int i, int j, double value){element[i][j] = value;}

double Matrix::getEle(int i, int j)	{return element[i][j];}

void Matrix::Print()
{
	for(int i=0; i<row; i++)
	{
		for(int j=0; j<col; j++)
			cout<<element[i][j]<<"\t";
	cout<<endl;
	}
	cout<<endl;
}

double Matrix::Determinant()
{
	double det =0;
	int factor = 0;
	if(row==2 && col==2)	//base case of rank 2, lessens number of recursions needed
		det = element[0][0]*element[1][1]-element[0][1]*element[1][0];
	else
	{
		Matrix A(row, col);
		Matrix B(row-1, col-1);
		for(int i=0; i<row; i++)
			for(int j=0; j<col; j++)
				A.setEle(i,j,element[i][j]);
		for(int k=0; k<col; k++)
		{
			B = A.Reduce(0,k);
			if(k%2==0)
				factor=1;
			else
				factor=-1;
			det+=factor*element[0][k]*B.Determinant();
		}
	}
	return det;
	
}

Matrix Matrix::Reduce(int i, int j)
{
	Matrix tmp(row-1, col-1);

	for(int m=0; m<row; m++)
		for(int k=0; k<col; k++)
		{
			if(m<i && k<j)
				tmp.setEle(m,k,element[m][k]);
			else if(m>i && k<j)
				tmp.setEle(m-1,k,element[m][k]);
			else if(m<i && k>j)
				tmp.setEle(m,k-1,element[m][k]);
			else if(m>i && k>j)
				tmp.setEle(m-1,k-1,element[m][k]);
		}

	return tmp;
}
