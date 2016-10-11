// ChunkedFileReader.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <chrono>

using namespace std;

using ms = chrono::milliseconds;
using get_time = chrono::steady_clock ;

int main()
{
    auto start = get_time::now();

    FILE *pFile;

    pFile = fopen("F:\\Diablo III\\Data\\data\\data.001", "rb");
    if (pFile == NULL)
    {
        printf("Unable to open input file.\n");
        return 1;
    }

    printf("File opened\n");

    unsigned char pChunk[512];

    fseek(pFile, 0L, SEEK_END);
    long length = ftell(pFile);
    fseek(pFile, 0L, SEEK_SET);

    //printf("Pos: %ld\n", ftell(pFile));

    unsigned char* pBuffer = new unsigned char[length + 1];

    printf("Start reading chunks for %ld\n", length);

    unsigned long pos = 0L;
    while (pos < length)
    {
        size_t bytesRead = fread(pChunk, sizeof(unsigned char), 512, pFile);
        memcpy(pBuffer + pos, pChunk, bytesRead);

        //printf("Read %zd bytes at pos %ld\n", bytesRead, pos);

        pos += bytesRead;
    }

    printf("Done\n");

    fclose(pFile);

    delete pBuffer;

    auto end = get_time::now();
    auto diff = end - start;

    printf("Elapsed time is : %ld ms", chrono::duration_cast<ms>(diff).count());

    return 0;
}

