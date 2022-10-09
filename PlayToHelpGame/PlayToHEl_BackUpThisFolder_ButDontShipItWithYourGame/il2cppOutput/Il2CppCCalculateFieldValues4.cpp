#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <stdint.h>
#include <limits>



// System.String
struct String_t;



IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Attribute
struct Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA  : public RuntimeObject
{
};

// Doozy.DDebug
struct DDebug_t87E69A750901DB45F1FBFEA3587D1CADB1058B84  : public RuntimeObject
{
};

// System.Runtime.Serialization.DataContractAttribute
struct DataContractAttribute_tD065D7D14CC8AA548815166AB8B8210D1B3C699F  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.Boolean System.Runtime.Serialization.DataContractAttribute::isReference
	bool ___isReference_0;
};

// System.Runtime.Serialization.DataMemberAttribute
struct DataMemberAttribute_t8AE446BE9032B9BC8E7B2EDC785F5C6FA0E5BB73  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.String System.Runtime.Serialization.DataMemberAttribute::name
	String_t* ___name_0;
	// System.Int32 System.Runtime.Serialization.DataMemberAttribute::order
	int32_t ___order_1;
	// System.Boolean System.Runtime.Serialization.DataMemberAttribute::isRequired
	bool ___isRequired_2;
	// System.Boolean System.Runtime.Serialization.DataMemberAttribute::emitDefaultValue
	bool ___emitDefaultValue_3;
};

// System.Runtime.Serialization.EnumMemberAttribute
struct EnumMemberAttribute_t65B5E85E642C96791DD6AE5EAD0276350954126F  : public Attribute_tFDA8EFEFB0711976D22474794576DAF28F7440AA
{
	// System.String System.Runtime.Serialization.EnumMemberAttribute::value
	String_t* ___value_0;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable12001[1] = 
{
	static_cast<int32_t>(offsetof(DataContractAttribute_tD065D7D14CC8AA548815166AB8B8210D1B3C699F, ___isReference_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable12002[4] = 
{
	static_cast<int32_t>(offsetof(DataMemberAttribute_t8AE446BE9032B9BC8E7B2EDC785F5C6FA0E5BB73, ___name_0)),static_cast<int32_t>(offsetof(DataMemberAttribute_t8AE446BE9032B9BC8E7B2EDC785F5C6FA0E5BB73, ___order_1)),static_cast<int32_t>(offsetof(DataMemberAttribute_t8AE446BE9032B9BC8E7B2EDC785F5C6FA0E5BB73, ___isRequired_2)),static_cast<int32_t>(offsetof(DataMemberAttribute_t8AE446BE9032B9BC8E7B2EDC785F5C6FA0E5BB73, ___emitDefaultValue_3)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable12003[1] = 
{
	static_cast<int32_t>(offsetof(EnumMemberAttribute_t65B5E85E642C96791DD6AE5EAD0276350954126F, ___value_0)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable12010[2] = 
{
	0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable12014[4] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,};
