
# #!/bin/bash
# # full-pipeline.sh — Final, Perfect Local Azure DevOps Simulation
# set -euo pipefail

# # ── Generate unique, safe image tag ────────────────────────
# BRANCH=$(git rev-parse --abbrev-ref HEAD | tr '[:upper:]' '[:lower:]' | sed 's/[^a-z0-9._-]/_/g')
# BUILD_ID=$(date +%s)
# TAG="${BRANCH}-build${BUILD_ID}"
# IMAGE_NAME="myapp:$TAG"

# echo "============================================================"
# echo "LOCAL AZURE DEVOPS CI/CD PIPELINE – FINAL VERSION"
# echo "Branch : $BRANCH"
# echo "Image  : $IMAGE_NAME"
# echo "============================================================"

# # 1. Build & Test .NET app
# echo "1. Build Stage: Restore → Build → Test"
# dotnet restore
# dotnet build --no-restore -c Release
# dotnet test --no-build -c Release --verbosity minimal

# # 2. Build Docker image directly inside Minikube (no push needed!)
# echo "2. Building Docker image inside Minikube → $IMAGE_NAME"
# eval $(minikube docker-env)
# docker build -t "$IMAGE_NAME" -f MyApp/Dockerfile .
# eval $(minikube docker-env -u)

# # 3. Deploy to Dev (automatic)
# echo "3. Deploying to Dev (automatic)"
# sed "s|IMAGE_TAG|$IMAGE_NAME|g" k8s/deployment.yaml | kubectl apply -f - -n dev
# kubectl apply -f k8s/service.yaml -n dev
# kubectl set env deployment/myapp-deployment -n dev ASPNETCORE_ENVIRONMENT=Development

# # 4. Test – Manual Approval
# read -p "Approve deployment to TEST environment? (y/n): " approve
# if [[ "$approve" =~ ^[Yy]$ ]]; then
#   echo "Deploying to Test"
#   sed "s|IMAGE_TAG|$IMAGE_NAME|g" k8s/deployment.yaml | kubectl apply -f - -n test
#   kubectl apply -f k8s/service.yaml -n test
#   kubectl set env deployment/myapp-deployment -n test ASPNETCORE_ENVIRONMENT=Production
# else
#   echo "Test deployment skipped."
#   exit 0
# fi

# # 5. Prod – Manual Approval
# read -p "Approve deployment to PRODUCTION environment? (y/n): " approve
# if [[ "$approve" =~ ^[Yy]$ ]]; then
#   echo "Deploying to Production"
#   sed "s|IMAGE_TAG|$IMAGE_NAME|g" k8s/deployment.yaml | kubectl apply -f - -n prod
#   kubectl apply -f k8s/service.yaml -n prod
#   kubectl set env deployment/myapp-deployment -n prod ASPNETCORE_ENVIRONMENT=Production
#   echo "SUCCESS: Application successfully promoted to Production!"
# else
#   echo "Production deployment skipped."
#   exit 0
# fi

# # 6. Final verification
# echo "Waiting 15s for pods to start..."
# sleep 15

# echo -e "\n=== FINAL DEPLOYMENT STATUS ==="
# echo "DEV namespace:"
# kubectl get pods -n dev -l app=myapp -o wide
# echo ""
# echo "TEST namespace:"
# kubectl get pods -n test -l app=myapp -o wide
# echo ""
# echo "PROD namespace:"
# kubectl get pods -n prod -l app=myapp -o wide

# echo -e "\n=== IMAGE USED IN ALL ENVIRONMENTS ==="
# for ns in dev test prod; do
#   echo -n "$ns: "
#   kubectl get deployment myapp-deployment -n $ns -o jsonpath='{.spec.template.spec.containers[0].image}' 2>/dev/null || echo "Not deployed"
# done

# echo -e "\n=== ENVIRONMENT SETTINGS ==="
# for ns in dev test prod; do
#   echo -n "$ns: "
#   kubectl exec -n $ns deployment/myapp-deployment -- printenv ASPNETCORE_ENVIRONMENT 2>/dev/null || echo "Not set"
# done

# echo -e "\n=== TEST YOUR APP ==="
# echo "Dev environment (has Swagger):"
# echo "  kubectl port-forward -n dev deployment/myapp-deployment 8080:8080"
# echo "  Then open: http://localhost:8080/health"
# echo ""
# echo "Or use Minikube services:"
# echo "  minikube service myapp-service -n dev"
# echo "  minikube service myapp-service -n test"
# echo "  minikube service myapp-service -n prod"

# echo -e "\nPIPELINE COMPLETED SUCCESSFULLY!"
# echo      "Azure DevOps pipeline 100% locally."
# echo "============================================================"