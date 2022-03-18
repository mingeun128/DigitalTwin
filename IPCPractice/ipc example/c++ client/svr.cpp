#include <tchar.h>
#include <windows.h>
#include <iostream>
#include <string>
#include <conio.h>
#include <tchar.h>
#include <thread>
using namespace std;
#define BUFSIZE 100
char buf[100];
int getData();
int _tmain(int argc, _TCHAR* argv[])
{
	cout << "CLIENT" << endl;
	
	HANDLE hPipe;
	LPTSTR lpvMessage;
	BOOL   fSuccess = FALSE;
	DWORD  cbToWrite, cbWritten, dwMode;
	LPTSTR lpszPipename = TEXT("\\\\.\\pipe\\DEVSPipe");
	

	if (argc > 1)
		lpvMessage = argv[1];
	thread t1(getData);
	while (1)
	{
		hPipe = CreateFile(
			lpszPipename,   // pipe name 
			GENERIC_READ |  // read and write access 
			GENERIC_WRITE,
			0,              // no sharing 
			NULL,           // default security attributes
			OPEN_EXISTING,  // opens existing pipe 
			0,              // default attributes 
			NULL);          // no template file 

							// Break if the pipe handle is valid. 

		if (hPipe != INVALID_HANDLE_VALUE)
			break;


		// Exit if an error other than ERROR_PIPE_BUSY occurs. 

		if (GetLastError() != ERROR_PIPE_BUSY)
		{
			_tprintf(TEXT("Could not open pipe. GLE=%d\n"), GetLastError());
			return -1;
		}

		// All pipe instances are busy, so wait for 20 seconds. 

		if (!WaitNamedPipe(lpszPipename, 20000))
		{
			printf("Could not open pipe: 20 second wait timed out.");
			return -1;
		}
		
	}

	// Send a message to the pipe server.
	string str;
	while (true) {
		cin >> str;
		cbToWrite = (strlen(str.c_str())+1)*sizeof(TCHAR);
		printf("Sending %d byte message: \"%s\"\n", cbToWrite,	str.c_str());
		fSuccess = WriteFile(
			hPipe,                  // pipe handle 
			str.c_str(),             // message 
			cbToWrite,              // message length 
			&cbWritten,             // bytes written 
			NULL);                  // not overlapped 

		if (!fSuccess)
		{
			printf("WriteFile to pipe failed. GLE=%d\n", GetLastError());
			return -1;
		}
	}

	CloseHandle(hPipe);
	t1.join();

	return 0;
}
int getData() 
{
	HANDLE hPipe2;
	HANDLE hHeap = GetProcessHeap();
	DWORD  cbRead = 0;
	BOOL   fSuccess2 = FALSE;
	LPTSTR chBuf = (TCHAR*)HeapAlloc(hHeap, 0, BUFSIZE * sizeof(TCHAR));
	LPTSTR lpszPipename2 = TEXT("\\\\.\\pipe\\DEVSPipe2");
	while (1)
	{
		hPipe2 = CreateFile(
			lpszPipename2,   // pipe name 
			GENERIC_READ |  // read and write access 
			GENERIC_WRITE,
			0,              // no sharing 
			NULL,           // default security attributes
			OPEN_EXISTING,  // opens existing pipe 
			0,              // default attributes 
			NULL);          // no template file 
		if (hPipe2 != INVALID_HANDLE_VALUE)
			break;

		if (!WaitNamedPipe(lpszPipename2, 20000))
		{
			printf("Could not open pipe: 20 second wait timed out.");
			return -1;
		}
	}
	while (1) 
	{
		memset(chBuf, 0, BUFSIZE * sizeof(TCHAR));
		fSuccess2 = ReadFile(
			hPipe2,    // pipe handle 
			chBuf,    // buffer to receive reply 
			BUFSIZE * sizeof(TCHAR),  // size of buffer 
			&cbRead,  // number of bytes read 
			NULL);    // not overlapped 

		if (!fSuccess2 && GetLastError() != ERROR_MORE_DATA) {
			_tprintf(TEXT("GLE=%d\n"), GetLastError());
			break;
		}
		printf("Yello car Position is : %s \n", chBuf);
		//_tprintf(TEXT("%s\n"), chBuf);
		
	}
	CloseHandle(hPipe2);
	HeapFree(hHeap, 0, chBuf);
	FlushFileBuffers(hPipe2);
	DisconnectNamedPipe(hPipe2);
	CloseHandle(hPipe2);
	return 0;
}
