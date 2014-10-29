//Imran Yafai
//Computational Physics Project 8A
//Vector Class
#include <iostream>
#include <stdio.h>

using namespace std;

class Vector
{
	private:
		double x;
		double y;
		double z;
	public:
		Vector();
		Vector(double, double, double);
		~Vector();
		double getX();
		double getY();
		double getZ();

		void setX(double);
		void setY(double);
		void setZ(double);
		double Dot(Vector);
		Vector Cross(Vector);
		Vector Scalar(double);
		void Print();
};

Vector::Vector()
{
	x = 0;
	y = 0;
	z = 0;
}

Vector::Vector(double x1, double x2, double x3)
{
	x = x1;
	y = x2;
	z = x3;
}

Vector::~Vector() {}

double Vector::getX()	{return x;}
double Vector::getY()	{return y;}
double Vector::getZ()	{return z;}

void Vector::setX(double x1)	{x = x1;}
void Vector::setY(double x2)	{y = x2;}
void Vector::setZ(double x3)	{z = x3;}

double Vector::Dot(Vector B)
{
	double dot = 0;
	dot = x*B.getX()+y*B.getY()+z*B.getZ();
	return dot;
}

Vector Vector::Cross(Vector B)
{
	Vector tmp;
	double crossX = 0, crossY=0, crossZ=0;
	crossX = y*B.getZ()-z*B.getY();
	crossY = z*B.getX()-x*B.getZ();
	crossZ = x*B.getY()-y*B.getX();

	tmp.setX(crossX);
	tmp.setY(crossY);
	tmp.setZ(crossZ);

	return tmp;
}

Vector Vector::Scalar(double C)
{
	Vector tmp;
	tmp.setX(C*x);
	tmp.setY(C*y);
	tmp.setZ(C*z);

	return tmp;
}
void Vector::Print()	
{ cout<<x<<"i + "<<y<<"j + "<<z<<"k";}