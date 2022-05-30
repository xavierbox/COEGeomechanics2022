#include "stdafx.h"
#include "Utils.h"



 void Utils::pause() { getchar(); }// std::cout<<"pause...";char c; std::cin>>c;}

 bool  Utils::findSectionInFile( ifstream &file, string key )
	{
	std::transform( key.begin(), key.end(), key.begin(), ::tolower );

	string s;
	std::streampos initialPosition = file.tellg();
	std::transform( key.begin(), key.end(), key.begin(), ::tolower );

	std::streampos pos = file.tellg();
	bool retValue = false;
	for (int k = 0; k < 2; k++)
		{
		while (file >> s)
			{
			//std::cout << "Read: " << s << std::endl;
			std::transform( s.begin(), s.end(), s.begin(), ::tolower );
			if ((s[0] != '#') && (s == key))
				{
				k = 10;
				retValue = true;
				break;
				}
			else
				{
				pos = file.tellg();
				}
			}//while

		if (retValue == false)
			{
			file.clear();
			file.seekg( 0 );
			//std::cout << "Not Found in pass " << k << std::endl;

			}
		//else
		//std::cout << "Found in pass " << k << std::endl;


		}

	if (retValue == false)
		{
		file.clear();
		file.seekg( initialPosition );
		}


	return retValue;
	}



 std::vector<std::string> Utils::tokenize( std::string str, std::string delimiters )
	{
	std::vector<string> tokens;

	// Skip delimiters at beginning.
	string::size_type lastPos = str.find_first_not_of( delimiters, 0 );
	// Find first "non-delimiter".
	string::size_type pos = str.find_first_of( delimiters, lastPos );

	while (string::npos != pos || string::npos != lastPos)
		{
		// Found a token, add it to the vector.
		tokens.push_back( str.substr( lastPos, pos - lastPos ) );
		// Skip delimiters.  Note the "not_of"
		lastPos = str.find_first_not_of( delimiters, pos );
		// Find next "non-delimiter"
		pos = str.find_first_of( delimiters, lastPos );
		}

	return tokens;
	}

 std::vector<std::string> Utils::split( std::string str, std::string delimiters )
	{
	return Utils::tokenize( str, delimiters );
	}

  std::string Utils::toUpper( std::string str )
	{
	std::string data = str;
	std::transform( data.begin(), data.end(), data.begin(), ::tolower );
	return data;
	}

 void Utils:: removeSpace( std::string &s )
	{
	s.erase( std::remove_if( s.begin(), s.end(), ::isspace ), s.end() );
	}

 void Utils::getFolderAndFile( const string& str, string &folder, string &file )
	{
	size_t found;
	found = str.find_last_of( "/\\" );
	folder = str.substr( 0, found );
	file = str.substr( found + 1 );
	}

