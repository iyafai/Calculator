#include<iostream>
#include<math.h>

using namespace std;

class Matrix
{
	public:
		double **matrix;
		int M,N;

		Matrix(int m, int n);
		~Matrix();
		Matrix whatsLeft(int i, int j);
		void printMatrix();
		double det();
		
};

Matrix::Matrix(int m, int n)
{
	M = m;
	N = n;
	matrix = new double *[M];
	for(int i = 0; i < M; i++)
	{
		matrix[i] = new double [N];
	}
}

void Matrix::printMatrix()
{
	for(int i = 0; i < M; i++)
		for(int j = 0; j < N; j++)
		{
			if(j == N-1)
			{
				cout << matrix[i][j] << endl;
			}
			else
			{
				cout << matrix[i][j] << " , ";
			}
		}
}
double Dot(Matrix & A, Matrix & B, int i, int j)
{
	double result = 0;
	for(int k = 0; k < A.N; k++)
	{
		result += A.matrix[i][k]*B.matrix[k][j];
	}
	return result;
}
void Mult(Matrix & A, Matrix & B, Matrix & C)
{
	if(A.N != B.M)
	{
		cout << "Matrix multiplication not possible: N != M" << endl;
	}
	else
	{
		for(int i = 0; i < A.M; i++)
		{
			for(int j = 0; j < B.N; j++)
			{
				C.matrix[i][j] = Dot(A, B, i, j);
			}
		}
	}
}

void Add(Matrix & m, int i, int j, double factor)
{
	for(int k = 0; k < m.M; k++)
		m.matrix[i][k] = m.matrix[i][k] + factor*m.matrix[j][k];
}
void ScalMult(Matrix & m, int j, double factor)
{
	for(int k = 0; k < m.M; k++)
		m.matrix[j][k] = factor*m.matrix[j][k];
}

Matrix transpose(Matrix & m)
{
	Matrix tmp(m.N, m.M);
	int k = 0;
	for(int i = 0; i < m.M; i++)
	{
		int l = 0;
		for(int j = 0; j < m.N; j++)
		{
			tmp.matrix[l][k] = m.matrix[i][j];
			l++;
		}
		k++;
	}
	return tmp;
}
			
void inverse(Matrix & m, Matrix & inv)
{
	Matrix copy = m;
	if(m.M != m.N)
	{	
		cout << "Must be a square matrix." << endl;
	}
	else
	{
		for(int j = 0; j < m.N; j++)
		{
			double div = 1/copy.matrix[j][j];
			ScalMult(inv,j,div);
			ScalMult(copy,j,div);
			for(int i = 0; i < m.M; i++)
			{
				if(i != j)
				{
					double factor = copy.matrix[i][j]/copy.matrix[j][j];
					Add(copy,i,j,-1*factor);
					Add(inv,i,j,-1*factor);
				}
			}
		}
	}
}

Matrix Matrix::whatsLeft(int i, int j)
{
	Matrix tmp(M-1,N-1);
	int c = 0;
	for(int a = 0; a < M; a++)
	{
		int d = 0;
		for(int b = 0; b < N; b++)
		{
			if(a == i || b == j)
			{
			}
			else
			{
				tmp.matrix[c][d] = matrix[a][b];
				d++;
			}
		}
		if(a != i)
			c++;
	}
	return tmp;
}
double det(Matrix & m)
{
	double result = 0;
	for(int j = 0; j < m.N; j++)
	{
		if(m.M > 2)
		{
			if(m.matrix[0][j] != 0)
			{
				Matrix tmp = m.whatsLeft(0,j);
				result += m.matrix[0][j]*det(tmp)*pow(-1,j);
				for(int i = 0; i < m.M-1; i++)
					delete [] tmp.matrix[i];
				delete [] tmp.matrix;
			}

		}
		else
		{	
			return (m.matrix[0][0]*m.matrix[1][1]-m.matrix[0][1]*m.matrix[1][0]);
		}
	}
	return result;
}
			
Matrix::~Matrix()
{}
