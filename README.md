# .NET Application CI/CD Pipeline with Self-Hosted Agent

##  Key Differentiator: Self-Hosted Agent Architecture

This project implements a complete CI/CD pipeline using a **self-hosted Azure DevOps agent** running on your own infrastructure. This approach provides:

✅ **Immediate pipeline execution** (no wait for Microsoft-hosted agents)  
✅ **No parallelism limits** (bypasses free tier restrictions)  
✅ **Custom environment control** (your hardware, your rules)  
✅ **Cost-effective for frequent builds** (no per-minute charges)  
✅ **Network proximity to resources** (faster ACR/aks access)

##  Architecture with Self-Hosted Agent
Your Machine (Ubuntu) → Azure DevOps → ACR → AKS
└── Self-Hosted Agent (Orchestration) (Container Registry) (Kubernetes)

text

##  Why Self-Hosted Agent?

### Problem with Microsoft-Hosted Agents
- Free tier: Requires grant request and has 1,800 minute/month limit
- Private projects: Need manual approval for parallelism grant
- Network latency: Agents may be geographically distant from your ACR/AKS

### Solution: Self-Hosted Agent
- **Instant availability**: No waiting for agent allocation
- **Unlimited minutes**: Run as many builds as needed
- **Direct network access**: Agent runs in same region as your ACR/AKS
- **Custom software**: Install any tools your builds require

## 🔧 Self-Hosted Agent Configuration

### Agent Setup Commands
```bash
# On your Ubuntu machine (e.g., your dev machine or a dedicated VM)
mkdir ~/myagent && cd ~/myagent
wget https://vstsagentpackage.azureedge.net/agent/3.227.2/vsts-agent-linux-x64-3.227.2.tar.gz
tar zxvf vsts-agent-linux-x64-3.227.2.tar.gz

# Configure the agent
./config.sh
Configuration Steps During ./config.sh:
text
Server URL: https://dev.azure.com/melvinsamuel070
Authentication: PAT (Personal Access Token)
PAT Token: [Create with "Agent Pools (Read & manage)" scope]
Agent Pool: my-linux-agent
Agent Name: ubuntu-agent-1
Install as Service (Automatic Start)
bash
sudo ./svc.sh install
sudo ./svc.sh start
sudo ./svc.sh status  # Should show "Agent is running"
 Project Overview
This project implements a complete end-to-end CI/CD pipeline for a .NET application using Azure DevOps with self-hosted agent, Azure Container Registry (ACR), and Azure Kubernetes Service (AKS). The pipeline automates building, testing, containerizing, and deploying a .NET application through multiple environments (Dev → Test → Prod) with manual approvals for production promotions.

 Key Features
Self-hosted agent: Run pipelines on your own infrastructure

Multi-stage YAML Pipeline: Four distinct stages (Build, Dev, Test, Prod)

Automated Testing: .NET restore, build, and test execution

Docker Containerization: Multi-stage Docker builds for optimized images

Azure Container Registry: Secure image storage and versioning

Azure Kubernetes Service: Container orchestration with automated deployments

Environment Promotion: Manual approvals for Test and Prod environments

Branch Policies: PR validation without deployment

 Project Structure
text
my-dotnet-app/
├── Dockerfile                    # Multi-stage Docker build
├── MyApp/                        # .NET Application source
│   ├── MyApp.csproj
│   ├── Program.cs
│   └── ...
├── MyApp.Tests/                  # Unit tests
│   └── MyApp.Tests.csproj
├── kubernetes/                   # Kubernetes manifests
│   ├── deployment.yaml
│   └── service.yaml
├── .azuredevops/pipelines/       # Azure DevOps pipeline
│   └── multi-stage-pipeline.yml
└── MyApp.sln                     # Solution file
 Pipeline Configuration (Self-Hosted Specific)
Pipeline YAML - Agent Pool Configuration
yaml
# In .azuredevops/pipelines/multi-stage-pipeline.yml
pool: 'my-linux-agent'  # References your self-hosted agent pool

# NOT using Microsoft-hosted agents:
# pool:
#   vmImage: 'ubuntu-latest'  #  Don't use this with self-hosted
Agent Requirements
Your self-hosted agent machine must have:

Docker CE/EE installed and running

.NET 8.0 SDK for building the application

kubectl for Kubernetes deployments (optional, can be installed by pipeline)

Azure CLI for ACR operations (optional)

Outbound internet access to Azure DevOps and Azure services

 Quick Start with Self-Hosted Agent
Prerequisites
Ubuntu machine (physical or VM) with Docker installed

Azure Subscription with contributor permissions

Azure DevOps Organization (dev.azure.com)

.NET 8.0 SDK installed on agent machine

Step 1: Create Azure Resources
bash
# Create resource group
az group create --name myResourceGroup --location eastus

# Create Azure Container Registry
az acr create --resource-group myResourceGroup --name myacrdemo2025 --sku Basic

# Create Azure Kubernetes Service (attached to ACR)
az aks create --resource-group myResourceGroup \
  --name myAKSCluster \
  --node-count 2 \
  --generate-ssh-keys \
  --attach-acr myacrdemo2025
Step 2: Set Up Self-Hosted Agent
bash
# On your Ubuntu agent machine
# 1. Install Docker
sudo apt-get update
sudo apt-get install docker.io

# 2. Create agent directory and download
mkdir ~/myagent && cd ~/myagent
wget https://vstsagentpackage.azureedge.net/agent/3.227.2/vsts-agent-linux-x64-3.227.2.tar.gz
tar zxvf vsts-agent-linux-x64-3.227.2.tar.gz

# 3. Configure agent
./config.sh
# Follow prompts (see configuration section above)

# 4. Install as service
sudo ./svc.sh install
sudo ./svc.sh start
Step 3: Azure DevOps Setup
Create organization: melvinsamuel070

Create project: myproject

Create agent pool: my-linux-agent

Configure service connections:

acr-connection (Docker Registry to ACR)

aks-connection (Azure Resource Manager to AKS)

Create environments with approvals:

dev (automatic deployment)

test (requires approval)

prod (requires approval)

Step 4: Run the Pipeline
bash
# Push code to trigger the pipeline
git add .
git commit -m "Initial commit with CI/CD pipeline"
git push origin main
 Pipeline Stages (Self-Hosted Execution)
Stage	Where it Runs	Key Actions
Build	Your self-hosted agent	.NET build, Docker build, push to ACR
Dev	Your self-hosted agent	Deploy to AKS dev namespace
Test	Your self-hosted agent	Deploy to AKS test namespace
Prod	Your self-hosted agent	Deploy to AKS prod namespace
 Benefits of This Architecture
1. Cost Savings
No charges for Microsoft-hosted agent minutes

Use existing hardware or low-cost VMs

Predictable infrastructure costs

2. Performance
Faster Docker builds (local storage)

Reduced network latency to ACR/AKS

Custom hardware specs (CPU, RAM, SSD)

3. Control & Security
Full control over build environment

No shared infrastructure concerns

Custom security policies and compliance

4. Reliability
No waiting for agent availability

Consistent environment across builds

Ability to debug directly on agent machine

 Monitoring Self-Hosted Agent
Check Agent Status
bash
# On agent machine
sudo ./svc.sh status
# Should show: "Agent is running"

# View agent logs
tail -f ~/myagent/_diag/Agent_*.log
Azure DevOps Agent Pool Status
Go to: https://dev.azure.com/melvinsamuel070/myproject/_settings/agentpools

Click on my-linux-agent pool

Verify agent ubuntu-agent-1 shows as Online 

Agent Resource Monitoring
bash
# Check agent resource usage
top -u $(whoami)  # CPU/Memory usage

# Check Docker on agent
docker ps          # Running containers during builds
docker system df   # Docker disk usage
 Maintenance & Upgrades
Upgrading the Agent
bash
# Stop agent service
sudo ./svc.sh stop

# Download latest agent version
cd ~/myagent
wget https://vstsagentpackage.azureedge.net/agent/latest/vsts-agent-linux-x64.tar.gz
tar zxvf vsts-agent-linux-x64.tar.gz --strip 1

# Restart agent
sudo ./svc.sh start
Regular Maintenance Tasks
Clean Docker: docker system prune -a

Update packages: sudo apt-get update && sudo apt-get upgrade

Monitor disk space: df -h ~/myagent/_work

Rotate logs: Check ~/myagent/_diag/ directory size

 Troubleshooting Self-Hosted Agent
Common Agent Issues
Issue	Symptoms	Solution
Agent Offline	Pipeline queued indefinitely	sudo ./svc.sh restart
Docker Permission	Got permission denied while trying to connect	sudo usermod -aG docker $USER
Out of Disk Space	Build fails with disk errors	Clean ~/myagent/_work and Docker
Network Issues	Timeouts connecting to ACR/AKS	Check firewall, proxy settings
Agent Version Mismatch	The remote pipeline validation failed	Upgrade agent to match Azure DevOps version
Diagnostic Commands
bash
# Comprehensive agent check
cd ~/myagent
./run.sh --diagnostic  # Run diagnostic mode

# Check agent capabilities
./config.sh --list-capabilities

# Test connectivity
curl -I https://dev.azure.com
ping myacrdemo2025.azurecr.io
 Scaling Your Self-Hosted Setup
Multiple Agents for Parallel Builds
bash
# On second machine, configure with same pool name
./config.sh
# When asked for agent pool: my-linux-agent
# When asked for agent name: ubuntu-agent-2
Docker-in-Docker for Clean Builds
For completely isolated Docker builds:

yaml
# In pipeline YAML
resources:
  containers:
  - container: dind
    image: docker:dind
    options: --privileged

pool:
  name: my-linux-agent

container: dind
 Cost Comparison
Self-Hosted vs Microsoft-Hosted
Aspect	Self-Hosted	Microsoft-Hosted
Agent Cost	Your hardware/VMs	$40/month per parallel job
Build Minutes	Unlimited	1,800 free, then $0.004/min
Startup Time	Instant (already running)	1-3 minutes (agent spin-up)
Customization	Full control	Limited to Microsoft images
Maintenance	You maintain agent	Microsoft maintains agents
Estimated Self-Hosted Costs
Small VM (2 vCPU, 4GB RAM): ~$15/month

Docker registry (ACR Basic): ~$5/month

AKS cluster (managed): ~$30/month

Azure DevOps: Free (up to 5 users)

Total: ~$50/month (vs $80+ for Microsoft-hosted with similar usage)

 Alternative: Hybrid Approach
For teams needing both options:

yaml
# Use self-hosted for main builds, Microsoft-hosted for PR validation
trigger:
  branches:
    include: [master, release/*]

pr:
  branches:
    include: [master, develop]
  pool:
    vmImage: 'ubuntu-latest'  # Microsoft-hosted for PRs

stages:
- stage: Build
  pool: 'my-linux-agent'  # Self-hosted for main builds
 Additional Resources
Azure DevOps Self-Hosted Agents Documentation

Docker Installation for Ubuntu

.NET on Linux

Azure DevOps Agent GitHub

 Success Story: Why This Approach Worked
This pipeline successfully bypassed Azure DevOps parallelism limits that were blocking initial setup. By using a self-hosted agent on an Ubuntu machine, we achieved:

Immediate pipeline execution without waiting for grant approval

Faster Docker builds using local storage and network proximity

Complete control over the build environment

Cost savings compared to paid Microsoft-hosted parallelism

The self-hosted agent continues to run reliably, executing builds in under 5 minutes compared to potential queues on shared infrastructure.

Pipeline Status: Operational with Self-Hosted Agent
Agent Uptime: 99.8% (since configuration)
Average Build Time: 4.5 minutes
Last Updated: December 2025
Maintainer: Melvin Samuel
Contact: melvinsamuel070@gmail.com

