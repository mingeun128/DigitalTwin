#include <tchar.h>
#include <windows.h>
#include <iostream>
#include <string>
#include <conio.h>
#include <tchar.h>
using namespace std;
#define BUFSIZE 100
char buf[100];

int _tmain(int argc, _TCHAR* argv[])
{
	cout << "CLIENT" << endl;
	
	HANDLE hPipe, hPipe2;
	HANDLE hHeap = GetProcessHeap();
	LPTSTR lpvMessage;
	BOOL   fSuccess = FALSE;
	BOOL   fSuccess2 = FALSE;
	DWORD  cbRead, cbToWrite, cbWritten, dwMode;
	TCHAR* chBuf = (TCHAR*)HeapAlloc(hHeap, 0, BUFSIZE * sizeof(TCHAR));
	LPTSTR lpszPipename = TEXT("\\\\.\\pipe\\DEVSPipe");
	LPTSTR lpszPipename2 = TEXT("\\\\.\\pipe\\DEVSPipe2");

	if (argc > 1)
		lpvMessage = argv[1];
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
		hPipe2 = CreateFile(
			lpszPipename2,   // pipe name 
			GENERIC_READ |  // read and write access 
			GENERIC_WRITE,
			0,              // no sharing 
			NULL,           // default security attributes
			OPEN_EXISTING,  // opens existing pipe 
			0,              // default attributes 
			NULL);          // no template file 

		if (hPipe != INVALID_HANDLE_VALUE)
			break;
		if (hPipe2 != INVALID_HANDLE_VALUE)
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
		if (!WaitNamedPipe(lpszPipename2, 20000))
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

		fSuccess2 = ReadFile(
			hPipe2,    // pipe handle 
			chBuf,    // buffer to receive reply 
			BUFSIZE * sizeof(TCHAR),  // size of buffer 
			&cbRead,  // number of bytes read 
			NULL);    // not overlapped 

		if (!fSuccess2 && GetLastError() != ERROR_MORE_DATA)
			break;

		_tprintf(TEXT("\"%s\"\n"), chBuf);
		memset(chBuf, 0, BUFSIZE * sizeof(TCHAR));

		if (!fSuccess)
		{
			printf("WriteFile to pipe failed. GLE=%d\n", GetLastError());
			return -1;
		}
	}

	CloseHandle(hPipe);
	CloseHandle(hPipe2);

	return 0;
}
