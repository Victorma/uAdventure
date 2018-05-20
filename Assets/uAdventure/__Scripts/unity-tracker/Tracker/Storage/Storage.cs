/*
 * Copyright 2016 e-UCM (http://www.e-ucm.es/)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * This project has received funding from the European Union’s Horizon
 * 2020 research and innovation programme under grant agreement No 644187.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0 (link is external)
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

namespace RAGE.Analytics.Storages
{
	public interface Storage
	{
		void SetTracker (Tracker tracker);
		
		/// <summary>
		/// The tracker wants to start sending traces
		///</summary>
		void Start (Net.IRequestListener startListener);
		
		///<summary>
		/// The tracker wants to send the given data
		///</summary>
		void Send (String data, Net.IRequestListener flushListener);

		bool IsAvailable();
	}
}