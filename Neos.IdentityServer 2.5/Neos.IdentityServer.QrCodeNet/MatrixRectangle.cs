﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2011 George Mamaladze                                                                                                                                                      //
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//

using System;
using System.Collections;
using System.Collections.Generic;

namespace Neos.IdentityServer.MultiFactor.QrEncoding
{
    internal struct MatrixRectangle : IEnumerable<MatrixPoint>
    {
        public MatrixPoint Location { get; private set; }
        public MatrixSize Size { get; private set; }

        internal MatrixRectangle(MatrixPoint location, MatrixSize size) :
            this()
        {
            Location = location;
            Size = size;
        }

        public IEnumerator<MatrixPoint> GetEnumerator()
        {
            for (int j = Location.Y; j < Location.Y + Size.Height; j++)
            {
                for (int i = Location.X; i < Location.X + Size.Width; i++)
                {
                    yield return new MatrixPoint(i, j);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Format("Rectangle({0};{1}):({2} x {3})", Location.X, Location.Y, Size.Width, Size.Height);
        }
    }
}
