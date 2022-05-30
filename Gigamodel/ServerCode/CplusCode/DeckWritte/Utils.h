#ifndef _UTILS_H_ 
#define _UTILS_H_ 1



#include <cstdio>
#include <cstdlib>
#include <string.h>
#include <iostream>
#include <fstream>
#include <algorithm>
#include <vector>

#include "Definitions.h"


#ifdef _WINDOWS 
#include <Windows.h>
#include <direct.h>

#define getcwd _getcwd 
#else 
#include <dirent.h>
#include <unistd.h>

#endif



using namespace std;

class Utils
	{
	public:

		static void pause();

		static bool findSectionInFile( ifstream &file, string key );  //, bool comments)

		static std::vector<std::string> tokenize( std::string str, std::string delimiters );

		static std::vector<std::string> split( std::string str, std::string delimiters );

		static std::string toLower( std::string str );

		static std::string toUpper( std::string str );

		static void removeSpace( std::string &s );


		static void getFolderAndFile( const string& str, string &folder, string &file );

		static std::vector<std::string> tokenize2( std::string str, const char *letters ) 
			{
			std::vector<std::string> out; 
			char * destPtr = (char *)str.c_str();
			char *token = std::strtok( destPtr, letters );
			while (token != NULL) 
				{
				out.push_back( token ); 
				token = std::strtok( NULL, letters );
				}

			return out;
			}

	};




	#endif 

