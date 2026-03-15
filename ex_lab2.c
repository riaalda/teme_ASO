/*
Să se creeze un proiect în MVSC folosind limbajul C sau C++, în cadrul căruia se va proiecta o aplicație care citește toate datele 
dintr-un fișier al cărui path relativ este preluat de la intrarea standard și le va afișa la ieșirea standard. În cazul în care fișierul 
menționat nu există, se va afișa un mesaj de eroare. Codul va respecta notația maghiară.
*/
#include <stdio.h>

int main()
{
    char szPath[256];
    FILE *pFile;
    int iChar;

    // citire path relativ
    scanf("%255s", szPath);

    pFile = fopen(szPath, "r");

    if (pFile == NULL)
    {
        printf("Eroare: fisierul nu exista.\n");
        return 1;
    }

    while ((iChar = fgetc(pFile)) != EOF)
    {
        putchar(iChar);
    }

    fclose(pFile);

    return 0;
}
