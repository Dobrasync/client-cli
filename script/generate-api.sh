#!/bin/bash

# Stupid workaround as the -d param seems to not work. :(
TARGET_DIR="../src/Lamashare.CLI/Lamashare.CLI.ApiGen"
cd $TARGET_DIR

dotnet openapi-generator Lamashare.CLI.ApiGen -i "http://localhost:5127/swagger/v1/swagger.json" -n "Lamashare.CLI.ApiGen"
