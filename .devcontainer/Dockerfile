# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/main/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 8.0, 6.0, 3.1
ARG VARIANT="8.0"
FROM mcr.microsoft.com/dotnet/sdk:${VARIANT}

# [Optional] Install Node.js
ARG INSTALL_NODE="no"
ARG NODE_VERSION="lts/*"
RUN if [ "${INSTALL_NODE}" = "true" ]; then su vscode -c "source /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION}"; fi

# [Optional] Install Azure CLI
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# [Optional] Install additional tools
# RUN sudo apt-get update && sudo apt-get install -y <your-package-list>