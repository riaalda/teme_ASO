/* TEMĂ OBLIGATORIE
Să se citească toate subcheile unei chei (la alegere) din Registry și să se afișeze la ieșirea standard.*/
/* am ales cheie: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft */

#include <windows.h>
#include <stdio.h>

int main(void)
{
    HKEY hKey = NULL; // handle init
    const char *keyPath = "SOFTWARE\\Microsoft";

    // open key
    LONG result = RegOpenKeyA(
        HKEY_LOCAL_MACHINE,
        keyPath,
        &hKey
    );

    if (result != ERROR_SUCCESS) 
    {
        fprintf(stderr, "Error at opening the key : %ld\n", result);
        return 1;
    }

    printf("Subkeys for HKEY_LOCAL_MACHINE\\%s:\n\n", keyPath);

    // subkeys
    char  subKeyName[256];
    DWORD index = 0;

    while (1) 
    {

        result = RegEnumKeyA(
            hKey,           
            index,          
            subKeyName,     
            sizeof(subKeyName) // dim buffer
        );

        if (result == ERROR_NO_MORE_ITEMS)
            break;

        if (result != ERROR_SUCCESS) 
        {
            fprintf(stderr, "Error at enumeration (index %lu): %ld\n", index, result);
            break;
        }

        printf(" index - [%lu] subKeyName - %s\n", index, subKeyName);
        index++;
    }

    printf("Number of subkeys - %lu\n", index);

    
    LONG closeResult = RegCloseKey(hKey);
    if (closeResult != ERROR_SUCCESS) {
        fprintf(stderr, "Error at closing the key: %ld\n", closeResult);
        return 1;
    }

    printf("Key closed successfully\n");

    return 0;
}
