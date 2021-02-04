run-integration-tests: docker_files_path = docker/integration_tests

run-integration-tests:
	docker-compose -f ${docker_files_path}/docker-compose.yml build
	docker-compose -f ${docker_files_path}/docker-compose.yml run --rm tests
	
run-unit-tests:
	dotnet test src/DutyCycle.Tests
	
run-tests: run-integration-tests run-unit-tests
	