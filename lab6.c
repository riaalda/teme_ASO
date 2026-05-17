/*
TEMĂ OBLIGATORIE ❗ 
Să se proiecteze o aplicație (powershell, cmd, MSVC/C++ preferabil) 
care să identifice toate serviciile sistem care rulează la modul curent pe mașină.
*/

#include <windows.h>
#include <stdio.h>

int main() {
    SC_HANDLE hSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ENUMERATE_SERVICE);
    if (hSCManager==NULL) 
    {
        printf("Eroare - OpenSCManager : %lu\n", GetLastError());
        return 1;
    }

    DWORD bytesNeeded = 0;
    DWORD countServices = 0;

    // dimensiunea
    EnumServicesStatusEx(
        hSCManager,
        SC_ENUM_PROCESS_INFO,
        SERVICE_WIN32,
        SERVICE_STATE_ALL, 
        NULL,
        0,
        &bytesNeeded,
        &countServices,
        NULL,
        NULL
    );

    // alocare 
    BYTE* buffer = (BYTE*)malloc(bytesNeeded);
    if (buffer==NULL) 
    {
        printf("Eroare - alocare memorie\n");
        CloseServiceHandle(hSCManager);
        return 1;
    }

    LPENUM_SERVICE_STATUS_PROCESS services =
        (LPENUM_SERVICE_STATUS_PROCESS)buffer;

    if (!EnumServicesStatusEx( // EnumServicesStatusEx -> lista servicii, dupa filtrare cu SERVICE_RUNNING
            hSCManager,
            SC_ENUM_PROCESS_INFO,
            SERVICE_WIN32,
            SERVICE_STATE_ALL,
            buffer,
            bytesNeeded,
            &bytesNeeded,
            &countServices,
            NULL,
            NULL)) 
        {
            printf("Eroare - enumerare: %lu\n", GetLastError());
            free(buffer);
            CloseServiceHandle(hSCManager);
            return 1;
        }

    printf("Servicii care ruleaza:\n");
    printf("------------------------\n");

    for (DWORD i = 0; i < countServices; i++) 
    {
        if (services[i].ServiceStatusProcess.dwCurrentState == SERVICE_RUNNING) // active
        {
            wprintf(L"%s - %s \n", // LPWSTR unicode - wide L".." la wprintf
                   services[i].lpServiceName,
                   services[i].lpDisplayName);
        }
    }

    free(buffer);
    CloseServiceHandle(hSCManager);
    return 0;
}
