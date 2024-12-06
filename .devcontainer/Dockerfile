# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.238.1/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 8.0, 6.0, 3.1
ARG VARIANT="8.0-bullseye"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:${VARIANT}

# [Optional] Install Node.js
ARG INSTALL_NODE="true"
ARG NODE_VERSION="lts/*"
RUN if [ "${INSTALL_NODE}" = "true" ]; then su vscode -c "source /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION}"; fi

# [Optional] Install Azure CLI
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# [Optional] Install additional tools
# RUN sudo apt-get update && sudo apt-get install -y <your-package-list>