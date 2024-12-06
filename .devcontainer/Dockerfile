# Use the official .NET 8.0 SDK image
ARG VARIANT="8.0"
FROM mcr.microsoft.com/dotnet/sdk:${VARIANT}

# [Optional] Install Node.js
ARG INSTALL_NODE="true"
ARG NODE_VERSION="lts/*"
RUN if [ "${INSTALL_NODE}" = "true" ]; then \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs; \
    fi

# [Optional] Install Azure CLI
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# [Optional] Install additional tools
# RUN apt-get update && apt-get install -y <your-package-list>

# Set the working directory
# WORKDIR /workspace

# Copy the project files
# COPY . .

# Restore dependencies
# RUN dotnet restore

# Build the application
# RUN dotnet build --configuration Release --output /workspace/build

# Set the entry point for the container
# ENTRYPOINT ["dotnet", "/workspace/build/YourApp.dll"]