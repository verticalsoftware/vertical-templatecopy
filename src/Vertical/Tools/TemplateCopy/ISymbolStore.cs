// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents an object that acts like a symbol store
    /// </summary>
    public interface ISymbolStore
    {
        void Build();
        
        Func<string> GetValueFunction(string key);
    }
}