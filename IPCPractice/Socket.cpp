#pragma comment(lib, "ws2_32")

#include <iostream>
#include <WinSock2.h>
using namespace std;

#define PACKET_SIZE 1024
#define MAX 1

typedef struct _type {
	bool isTest;
	int das;
	float testFloat;
	char test[PACKET_SIZE];
}type;

int main() {
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) {
		cout << "WSA error";
		return 0;
	}

	SOCKET skt;
	skt = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (skt == INVALID_SOCKET) {
		cout << "socket error";
		closesocket(skt);
		WSACleanup();
		return 0;
	}

	SOCKADDR_IN addr = {};
	addr.sin_family = AF_INET;
	addr.sin_port = htons(50001);
	addr.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(skt, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR) {
		cout << "bind error";
		closesocket(skt);
		WSACleanup();
		return 0;
	}

	if (listen(skt, SOMAXCONN) == SOCKET_ERROR) {
		cout << "listen error";
		closesocket(skt);
		WSACleanup();
		return 0;
	}

	SOCKADDR_IN client = { 0 };
	int client_size = sizeof(client);
	SOCKET client_sock;
	client_sock = accept(skt, (SOCKADDR*)&client, &client_size);

	if (client_sock == INVALID_SOCKET) {
		cout << "accept error";
		closesocket(client_sock);
		closesocket(skt);
		WSACleanup();
		return 0;
	}

	type *tp = new type[MAX];
	//MAX만큼 동적할당 
	char buf[PACKET_SIZE];
	while (1) {
		for (int i = 0; i < MAX; i++)
		{
			cin >> (tp + i)->isTest >> (tp + i)->das >> (tp + i)->testFloat >> (tp + i)->test; // 구조체내용 입력 
		}
		for (int i = 0; i < MAX; i++)
			send(client_sock, (char*)&tp[i], PACKET_SIZE, 0); //차례대로 구조체정보 보내기 
	}
	closesocket(client_sock);
	closesocket(skt);
	WSACleanup();
	delete[] tp;

}