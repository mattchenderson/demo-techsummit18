az group create -n brk1155 -l eastus2
az group create -n brk2050 -l eastus

az group deployment create -g brk1155 -n brk1155-setup --template-file .\brk1155-template.json
az group deployment create -g brk2050 -n brk2050-setup --template-file .\brk2050-template.json

az group deployment show -g brk2050 -n brk2050-setup --query properties.outputs.ehcxnstr.value