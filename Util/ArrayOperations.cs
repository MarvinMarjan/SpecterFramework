using System;

namespace Specter.Util;


public static class Array
{
	public delegate void Operation<T>(ref T accumulator, T value); 


	/// <summary>
	/// Performs an operation for all items inside an array and returns the accumulator.
	/// </summary>
	/// <typeparam name="T">The base type.</typeparam>
	/// <param name="array">The array to perform operations.</param>
	/// <param name="initValue">The initial value of the accumulator.</param>
	/// <param name="calculationMethod">The custom operation to perform.</param>
	/// <returns></returns>
	public static T OperateAll<T>(this T[] array, T initValue, Operation<T> operation)
	{
		T accumulator = initValue;

		foreach (var item in array)
			operation(ref accumulator, item);

		return accumulator;
	}
}
