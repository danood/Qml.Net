#ifndef NET_TYPE_INFO_H
#define NET_TYPE_INFO_H

#include <QmlNet.h>
#include <QList>
#include <QString>
#include <QSharedPointer>
#include <QEnableSharedFromThis>

class NetMethodInfo;
class NetPropertyInfo;
class NetSignalInfo;

class NetTypeInfo : public QEnableSharedFromThis<NetTypeInfo> {
public:
    NetTypeInfo(QString fullTypeName);
    ~NetTypeInfo();

    QString getFullTypeName();

    QString getClassName();
    void setClassName(QString className);

    NetVariantTypeEnum getPrefVariantType();
    void setPrefVariantType(NetVariantTypeEnum variantType);

    void addMethod(QSharedPointer<NetMethodInfo> methodInfo);
    int getMethodCount();
    QSharedPointer<NetMethodInfo> getMethodInfo(int index);

    void addProperty(QSharedPointer<NetPropertyInfo> property);
    int getPropertyCount();
    QSharedPointer<NetPropertyInfo> getProperty(int index);

    void addSignal(QSharedPointer<NetSignalInfo> signal);
    int getSignalCount();
    QSharedPointer<NetSignalInfo> getSignal(int index);

    bool isLoaded();
    bool isLoading();
    void ensureLoaded();

    QMetaObject* metaObject;

private:
    QString _fullTypeName;
    QString _className;
    NetVariantTypeEnum _variantType;
    QList<QSharedPointer<NetMethodInfo>> _methods;
    QList<QSharedPointer<NetPropertyInfo>> _properties;
    QList<QSharedPointer<NetSignalInfo>> _signals;
    bool _lazyLoaded;
    bool _isLoading;
};

struct Q_DECL_EXPORT NetTypeInfoContainer {
    QSharedPointer<NetTypeInfo> netTypeInfo;
};

#endif // NET_TYPE_INFO_H
