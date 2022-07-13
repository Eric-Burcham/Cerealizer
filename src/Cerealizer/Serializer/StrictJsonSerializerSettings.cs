namespace Cerealizer
{
    using System.Globalization;
    using System.Runtime.Serialization;

    using Newtonsoft.Json;

    public class StrictJsonSerializerSettings : JsonSerializerSettings
    {
        public StrictJsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error;
        }

        public MissingTypeMemberHandling? MissingTypeMemberHandling { get; set; } = Cerealizer.MissingTypeMemberHandling.Error;

        internal bool HasCheckAdditionalContentValue => this.GetPrivateFieldValue<bool?>("_checkAdditionalContent") != null;

        internal bool HasConstructorHandlingValue => this.GetPrivateFieldValue<ConstructorHandling?>("_constructorHandling") != null;

        internal bool HasContextValue => this.GetPrivateFieldValue<StreamingContext?>("_context") != null;

        internal bool HasCultureValue => this.GetPrivateFieldValue<CultureInfo>("_culture") != null;

        internal bool HasDateFormatHandlingValue => this.GetPrivateFieldValue<DateFormatHandling?>("_dateFormatHandling") != null;

        internal bool HasDateFormatStringValue => this.GetPrivateFieldValue<bool>("_dateFormatStringSet");

        internal bool HasDateParseHandlingValue => this.GetPrivateFieldValue<DateParseHandling?>("_dateParseHandling") != null;

        internal bool HasDateTimeZoneHandlingValue => this.GetPrivateFieldValue<DateTimeZoneHandling?>("_dateTimeZoneHandling") != null;

        internal bool HasDefaultValueHandlingValue => this.GetPrivateFieldValue<DefaultValueHandling?>("_defaultValueHandling") != null;

        internal bool HasFloatFormatHandlingValue => this.GetPrivateFieldValue<FloatFormatHandling?>("_floatFormatHandling") != null;

        internal bool HasFloatParseHandlingValue => this.GetPrivateFieldValue<FloatParseHandling?>("_floatParseHandling") != null;

        internal bool HasFormattingValue => this.GetPrivateFieldValue<Formatting?>("_formatting") != null;

        internal bool HasMaxDepthValue => this.GetPrivateFieldValue<bool>("_maxDepthSet");

        internal bool HasMetadataPropertyHandlingValue => this.GetPrivateFieldValue<MetadataPropertyHandling?>("_metadataPropertyHandling") != null;

        internal bool HasMissingMemberHandlingValue => this.GetPrivateFieldValue<MissingMemberHandling?>("_missingMemberHandling") != null;

        internal bool HasNullValueHandlingValue => this.GetPrivateFieldValue<NullValueHandling?>("_nullValueHandling") != null;

        internal bool HasObjectCreationHandlingValue => this.GetPrivateFieldValue<ObjectCreationHandling?>("_objectCreationHandling") != null;

        internal bool HasPreserveReferencesHandlingValue => this.GetPrivateFieldValue<PreserveReferencesHandling?>("_preserveReferencesHandling") != null;

        internal bool HasReferenceLoopHandlingValue => this.GetPrivateFieldValue<ReferenceLoopHandling?>("_referenceLoopHandling") != null;

        internal bool HasStringEscapeHandlingValue => this.GetPrivateFieldValue<StringEscapeHandling?>("_stringEscapeHandling") != null;

        internal bool HasTypeNameAssemblyFormatHandlingValue => this.GetPrivateFieldValue<TypeNameAssemblyFormatHandling?>("_typeNameAssemblyFormatHandling") != null;

        internal bool HasTypeNameHandlingValue => this.GetPrivateFieldValue<TypeNameHandling?>("_typeNameHandling") != null;
    }
}
