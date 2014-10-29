#include <fstream>
#include <iostream>
#include <math.h>
#include <deque>
#include <time.h>

using namespace std;

struct node
{
	int num;
	int X;
	int Y;
};

double Distance_bw(node A, node B)
{
	double distance=0;
	double Xdif=A.X-B.X;
	double Ydif=A.Y-B.Y;
	double arg = Xdif*Xdif+Ydif*Ydif;
	distance = sqrt(arg);

	return distance;
}

/*deque<int> TripSummary(deque<node> A, int dim)
{
	deque<int> trip;
	for(int i=0; i<(dim); i++)
	{
		trip.push_back(A.at(i).num);
	}

	return trip;
}*/

deque<int> Initial(deque<int> A, int dim)
{
	deque<int> temp;
	int temp2;
	int mix=0;
	srand(time(NULL));
	for(int k=(dim-1); k>=0; k--)
	{
		if(k !=0)
		{
			mix = rand() % k;
			temp2=A.at(mix);
			A.erase(A.begin()+mix);
			temp.push_front(temp2);
		}
		else
		{
			temp2=A.at(0);
			A.erase(A.begin());
			temp.push_front(temp2);
		}
	}
	A.swap(temp);

	return A;
}

double Tot_Distance(deque<node> A, deque<int> trip, int dim)
{
	int current,next,Sum = 0;
	
	for(int k=0;k<(dim-1);k++)
	{
		current = trip.at(k);
		next = trip.at(k+1);
		Sum+=Distance_bw(A.at(current),A.at(next));
	}
	Sum+=Distance_bw(A.at(trip.at(dim-1)),A.at(trip.at(0)));
	
	return Sum;
}

deque<int> Swap_Node(deque<int> temp, int dim)
{
	deque<int> temp2;

	srand(time(NULL));
	int k = rand() % dim;
	int j = rand() % dim;
	int r = rand() % 4;

	temp2.push_back(temp.at(k));
	temp2.push_back(temp.at(k+1));
	temp2.push_back(temp.at(j));
	temp2.push_back(temp.at(j+1));


	temp.at(k+1)=temp2.at(3);
	temp.at(j+1)=temp2.at(1);
	
	temp2.clear();
	return temp;
}

void PrintTrip(deque<int> trip, deque<node> Map, int dim)
{
	double distance;
	distance = Tot_Distance(Map,trip,dim);
	cout<<distance<<": "<<Map.at(trip.at(0)).num;
	for(int i=1; i<(dim); i++)
		cout<<"->"<<(Map.at(trip.at(i))).num;
	cout<<"->"<<Map.at(trip.at(0)).num<<endl<<endl<<endl;
}

void Sim_Annealing(deque<int> trip, deque<node> Map, int dim, int iterations)
{
	srand(time(NULL));
	double k = 0;
	int P=0;
	int iter=0;
	double distance1, distance2, diff = 0, t=0;
	deque<int> temp;

	PrintTrip(trip, Map, dim);
	do
	{
		temp = Initial(trip, dim);
		distance1 = Tot_Distance(Map, trip, dim);
		distance2 = Tot_Distance(Map, temp, dim);
		diff = distance1-distance2;
		if(diff>0)
			trip.swap(temp);
		else
		{
			k=(rand() % 100)/100;
			t=-1*diff/(3*iter);
			P=exp(t);
			if(k<=P)
				trip.swap(temp);
		}
			
		temp.clear();
		iter++;
	}while(iter<iterations);
	cout<<"after "<<iter<<" iterations: "<<endl;
	PrintTrip(trip, Map, dim);
	
}

void main()
{	
	ifstream file1 ("TSPData.txt");
	int dim, n=0, m=0;
	deque<int> trip, T;
	deque<node> Map;
	node temp;
	double distance=0;
	file1>>dim;

	while(file1.good())		//pulls data from file1 and inserts into list 1
	{
		file1>> temp.num;
		trip.push_back((temp.num-1));
		file1>> temp.X;
		file1>> temp.Y;
		Map.push_back(temp);
	}

	//T = Inital(trip, dim);
	//Map.swap(NMap);
	//T = TripSummary(Map, dim);
	//trip.swap(T);


	//PrintTrip(trip, Map, dim);
	//T=Swap_Node(trip,dim);
	//PrintTrip(T,Map,dim);

//	T = Inital(trip, dim);
//	trip.swap(T);
	Sim_Annealing(trip, Map, dim, 2000);

	system("pause");
}