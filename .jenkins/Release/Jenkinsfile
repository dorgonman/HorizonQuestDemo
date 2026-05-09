// .jenkins/Release/Jenkinsfile — release publish entrypoint for HorizonQuestDemo.
@Library('jenkins-unreal-pipeline-library') _

unrealReleaseDeployPipeline(
    upstreamJob: 'HorizonPlugin/HorizonQuestDemo/Build/Development',
    ugsBuildJob: 'HorizonPlugin/HorizonQuestDemo/Build/UGSBuild'
)