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
		int getNumRow();
		int getNumCol();
		void setRow(int, double*);
		void setCol(int, double*);
		void Print();
		void PrintCom();
		Matrix Mult(Matrix);
		Matrix Sub(Matrix);
		Matrix ScalarMult(double);
		Matrix Transpose();
		Matrix Invert();
		Matrix Reduce(int, int);
		double Cofactor(int, int);
		double Determinant();
//		double* Eigenvalue();
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

int Matrix::getNumRow()	{return row;}

int Matrix::getNumCol()	{return col;}

void Matrix::setRow(int i, double* value)
{
	for(int j=0; j<col; j++)
		element[i][j]=value[j];
}

void Matrix::setCol(int j, double* value)
{
	for(int i=0; i<row; i++)
		element[i][j]=value[i];
}

void Matrix::Print()
{
	for(int i=0; i<row; i++)
	{
		for(int j=0; j<col; j++)
			cout<<element[i][j]<<" ";
	cout<<endl;
	}
	cout<<endl;
}

void Matrix::PrintCom()
{
	for(int i=0; i<row; i++)
	{
		for(int j=0; j<col; j++)
		{
			if(j<(col-1))
				cout<<element[i][j]<<",";
			else
				cout<<element[i][j];
		}
	cout<<endl;
	}
	cout<<endl;
}

Matrix Matrix::Mult(Matrix B)
{
	int colsecond = B.getNumCol();
	Matrix tmp(row, colsecond);	

	double value=0;


	for(int i=0; i < colsecond; i++)
	{
		for(int j=0; j<row; j++)
		{
			for(int m=0; m<col; m++)
				value+=element[j][m]*B.getEle(m,i);
			tmp.setEle(j,i,value);
			value=0;
		}
	}
	return tmp;
		
}

Matrix Matrix::Sub(Matrix B)
{
	Matrix tmp(row, col);	

	for(int i=0; i < row; i++)
	{
		for(int j=0; j<col; j++)
		{
			tmp.setEle(i,j,(element[i][j]-B.getEle(i,j)));
		}
	}
	return tmp;
}

Matrix Matrix::ScalarMult(double value)
{
	Matrix tmp(row, col);	

	for(int i=0; i < row; i++)
	{
		for(int j=0; j<col; j++)
		{
			tmp.setEle(i,j,(value*element[i][j]));
		}
	}
	return tmp;
}

Matrix Matrix::Transpose()
{
	Matrix tmp(col,row);
	for(int i=0; i<row; i++)
		for(int j=0; j<col; j++)
			tmp.setEle(j,i,element[i][j]);

	return tmp;
	
}

double Matrix::Determinant()
{
	double det =0;
	int factor = 0;
	if(row==1 && col==1)	//base case of rank 1
		det = element[0][0];
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

double Matrix::Cofactor(int i, int j)
{
	double C = 0;

	Matrix tmp(row, col);
	Matrix tmp2(row-1, col-1);
	for(int m=0; m<row; m++)
		for(int n=0; n<col; n++)
			tmp.setEle(m,n,element[m][n]);

	tmp2 = tmp.Reduce(i,j);

	int factor = 0;
	if((i+j)%2==0)
		factor=1;
	else
		factor=-1;
	C = factor*tmp2.Determinant();

	return C;
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
/*
double* Matrix::Eigenvalue()	//assuming no degeneracy
{
	Matrix tmp(row, col);
	Matrix tmp2(row, col);
	Matrix Identity(row, col);
	Matrix Lambda(row, col);

	double* eigenvalues = new double[row];
	double numEV = 0;

	for(int i=0; i<row; i++)
		for(int j=0; j<col; j++)
		{
			tmp.setEle(i,j,element[i][j]);
			tmp2.setEle(i,j,element[i][j]);
			Identity.setEle(i,i,1);
		}

	double det = tmp.Determinant();
	double det2;
	double det3;
	double lambda =0;
	double range = 10;

	while(numEV<row)
	{
		for(int m=-range; m<range; m+=.01)
		{
			if(det>0)
			{
				while(det2>0)
				{		
					Lambda = Identity.ScalarMult(range);
					tmp2 = tmp.Sub(Lambda);
					det2 = tmp2.Determinant();
					lambda+=.01;
				}
			}
			else if(det<0)
			{
				while(det2>0)
				{
					Lambda = Identity.ScalarMult(lambda);
					tmp2 = tmp.Sub(Lambda);
					det2 = tmp2.Determinant();
					lambda-=.01;
				}
			}
		}
	}
	return lambda;
}*/

Matrix Matrix::Invert()		//only works up to rank 8 or 9, after that gets to memory intensive or whatever
{				//should do something about that probably
//	cout<<"start"<<endl;
	Matrix A(row, col);
	double Idet = 0;
	double value = 0;
	for(int i=0; i<row; i++)
		for(int j=0; j<col; j++)
			A.setEle(i,j,element[i][j]);
	Matrix CT(row, col);
	Matrix AI(row, col);

	for(int k=0; k<row; k++)
	{
//		cout<<"A"<<endl;
		for(int m=0; m<col; m++)
		{
			value = A.Cofactor(k,m);
			CT.setEle(k,m,value);
		//	cout<<"CF"<<endl;
		}
	}
//	cout<<"a"<<endl;
	Idet = (1.0/(A.Determinant()));
	AI = CT.Transpose();
	AI = AI.ScalarMult(Idet);

	return AI;
}