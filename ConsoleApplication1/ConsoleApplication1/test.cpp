#include <iostream>
#include <stdlib.h>

using namespace std;
//using std::string; using std::cin; using std::cout;

int main()
{
	/*
	//---------------
	char* pArray[] = { "apple", "pear", "banana", "orange", "pineApple", "nnj" };
	std::cout << sizeof(pArray) << "$$$" << sizeof(*pArray) << std::endl;
	for (int i = 0; i < sizeof(pArray) / sizeof(*pArray); i++)
	{
		std::cout << pArray[i] << std::endl;
	}
	//----------------
	int i = 42;
	int* p1 = &i;
	*p1 = *p1 * *p1;
	std::cout << i << std::endl;
	//----------------
	int  i1 = 42;
	int *p;
	int *&r = p;
	r = &i1;
	*r = 0;
	std::cout << i1 << "#" << *p << "#" << *r << std::endl;
	//---------------
	int i2 = 42;
	int &r22 = i2;
	const int &r2 = i2;
	r22 = 1;
	std::cout << i2 << "#" << r22 << "#" << r2 << std::endl;
	//--------------
	const int i5 = 42;
	auto j5 = i5;
	const auto &k1 = i5;
	auto *p5 = &i5;
	const auto j6 = i5;
	//decltype((i5)) d;
	
	string line = "123";
	auto size = line.size();
	cout << size << endl;

	string g = line + "gh"+"sdfg";
	//string s = "asd" + "dfg" + line;//´íÎó ´Ó×óÍùÓÒ
	*/
	string s("Hello world!!!");
	decltype(s.size()) punct_cnt = 0;
	for (auto c : s)
		if (ispunct(c))
			++punct_cnt;
	cout << punct_cnt << endl;
	system("pause");
	return 0;
}