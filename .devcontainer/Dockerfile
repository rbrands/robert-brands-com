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
WORKDIR /workspace

# Copy the project files
COPY . .

# Restore dependencies
# RUN dotnet restore robert-brands-com.sln

# Build the application
# RUN dotnet build robert-brands-com.sln --configuration Release --output /workspace/build
# Expose the port that the application will run on
EXPOSE 5000
EXPOSE 5001

# Set the entry point for the container
ENTRYPOINT ["dotnet", "/workspace/build/robert-brands-com.dll"]