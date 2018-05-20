################################################################################
# Copyright (C) 2018 The e-UCM Learning Group
#
# Licensed under the Apache License, Version 2.0 (the "License"); you may not
# use this file except in compliance with the License.
#
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
# WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#
# See the License for the specific language governing permissions and
# limitations under the License.
################################################################################

$project_path = "$($pwd)"
$exchange_folder = "$($project_path)\\Exchange\\"
$guid_file = "$($exchange_folder)\\guidmap.csv"
$log_file = "$($exchange_folder)\\unity.log"
$guidmap_method = "uAdventure.Editor.FileIDUtil.GenerateGUIDMap"

$unity = "C:\\Program Files\\Unity\\Editor\\Unity.exe"
$arguments = "-batchmode -force-free -username $($env:license_username) -password $($env:license_password) -nographics -silent-crashes -logFile $($log_file) -projectPath $($project_path) -quit -executeMethod $($guidmap_method) $($guid_file)"

Write-Output "Creating Exchange dir"
New-Item -ItemType directory -Path $exchange_folder
Write-Output "Creating GUIDMap for $($project_path)"
$process = Start-Process $unity $arguments -Wait 

$error_code = 0
If ( $process.ExitCode -eq 0 ) {
    Write-Output "GUIDMap created at $($guid_file)."
}
Else {
    Write-Output "GUIDMap creation failed. Exited with $($process.ExitCode)."
    $error_code = $process.ExitCode
}

Write-Output 'Logs from build'
Get-Content $log_file
Write-Output 'Exchange dir:'
Get-ChildItem $exchange_folder

Write-Output "Finishing with code $($error_code)"
exit $error_code