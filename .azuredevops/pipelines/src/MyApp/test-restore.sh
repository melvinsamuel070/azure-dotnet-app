docker run --rm -it \
  -v $(pwd):/src \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  bash -c "
    cd /src && \
    dotnet nuget locals all --clear && \
    dotnet restore --verbosity detailed && \
    echo 'Checking packages...' && \
    find /root/.nuget/packages -name '*openapi*' -type d
  "
