using System;

namespace netcoreauth.model
{
	public enum ReturnCodes
	{
		ParameterError = 0,
		GenericException = 1,
		VerifySecretKeySucceeded = 2,
		VerifySecretKeyError = 3,
		DataCreateSucceeded = 100,
		DataCreatePartiallySucceeded = 101,
		DataUpdateSucceeded = 102,
		DataUpdatePartiallySucceeded = 103,
		DataRemoveSucceeded = 104,
		DataRemovePartiallySucceeded = 105,
		DataGetSucceeded = 106,
		DataCreateFailed = 200,
		DataCreateFailedWithDuplicateData = 201,
		DataCreateFailedWithErrorRelationships = 202,
		DataUpdateFailed = 203,
		DataUpdateFailedWithDuplicateData = 204,
		DataUpdateFailedWithErrorRelationships = 205,
		DataRemoveFailed = 206,
		DataGetFailed = 207,
		DataGetFailedWithErrorRelationships = 208,
		DataGetFailedWithNoData = 209,
	}

	public enum PlatformCode
	{
		iPhone = 1001,
		iPad = 1101,
		Android = 2001,
		AndroidPad = 2101,
		WindowsPhone = 3001,
		Windows = 4001,
	}

	public enum DealStatus
	{
		Draft = 10001,
		Submitted = 10002,
		Canceled = 10003,
		Accepted = 20001,
		Rejected = 20002,
		Paid = 20003,
		Progress = 30001,
		Exception = 30002,
		Refund = 30003,
		Refunded = 30004,
		Done = 40001,
		Evaluated = 50001,
		Closed = 60001,
	}
}
