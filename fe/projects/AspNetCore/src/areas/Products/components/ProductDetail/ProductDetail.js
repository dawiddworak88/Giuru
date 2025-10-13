import React, { Fragment, useEffect, useState } from "react";
import PropTypes from "prop-types";
import { Button } from "@mui/material";
import Files from "../../../../shared/components/Files/Files";
import Sidebar from "../../../../shared/components/Sidebar/Sidebar";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import Modal from "../../../../shared/components/Modal/Modal";
import { ExpandMore, ExpandLess } from "@mui/icons-material"
import { marked } from "marked";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";
import { Splide, SplideSlide } from "@splidejs/react-splide";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import ProductDetailModal from "../ProductDetailModal/ProductDetailModal";
import Price from "../../../../shared/components/Price/Price";
import { useOrderManagement } from "../../../../shared/hooks/useOrderManagement";
import QuantityCalculatorService from "../../../../shared/services/QuantityCalculatorService";
import Availability from "../../../../shared/components/Availability/Availability";
import CopyButton from "../../../../shared/components/CopyButton/CopyButton";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";

function ProductDetail(props) {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isImageModalOpen, setIsImageModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null);
    const [canActiveModal, setCanActiveModal] = useState(true);
    const [showMore, setShowMore] = useState(false);
    const [showMoreImages, setShowMoreImages] = useState(false);
    const [mediaItems, setMediaItems] = useState(props.mediaItems ? props.mediaItems.slice(0, 6) : []);
    const [activeMediaItemIndex, setActiveMediaItemIndex] = useState(0);
    const [cleanDescription, setCleanDescription] = useState('');
    const [plainText, setPlainText] = useState('');

    const {
        orderItems,
        addOrderItemToBasket
    } = useOrderManagement({
        initialBasketId: props.basketId ? props.basketId : null,
        initialOrderItems: props.orderItems ? props.orderItems : [],
        maxAllowedOrderQuantity: props.maxAllowedOrderQuantity,
        maxAllowedOrderQuantityErrorMessage: props.maxAllowedOrderQuantityErrorMessage,
        minOrderQuantityErrorMessage: props.minOrderQuantityErrorMessage,
        generalErrorMessage: props.generalErrorMessage,
        addProductToBasketMessage: props.toastSuccessAddProductToBasket,
        updateBasketUrl: props.updateBasketUrl,
        getPriceUrl: props.getProductPriceUrl
    });

    const handleAddOrderItemClick = (item) => {
        const quantity = parseInt(item.quantity);

        let product = {
            id: props.productId,
            sku: props.sku,
            name: props.title,
            stockQuantity: props.availableQuantity,
            outletQuantity: props.availableOutletQuantity
        };

        if (productVariant) {
            product = {
                id: productVariant.id,
                sku: productVariant.subtitle,
                name: productVariant.title,
                stockQuantity: productVariant.availableQuantity,
                outletQuantity: productVariant.availableOutletQuantity
            }
        }

        const getImageIds = () => {
            if (productVariant && Array.isArray(productVariant.images) && productVariant.images.length > 0) {
                return productVariant.images.map(img => img.id);
            }

            if (props.images && Array.isArray(props.images) && props.images.length > 0) {
                return props.images.map(img => img.id);
            }

            return [];
        };

        const getPriceInfo = () => {
            if (productVariant && productVariant.price) {
                return {
                    price: parseFloat(productVariant.price.current * quantity).toFixed(2),
                    currency: productVariant.price.currency
                };
            } else if (props.price) {
                return {
                    price: parseFloat(props.price.current * quantity).toFixed(2),
                    currency: props.price.currency
                };
            } else {
                return {
                    price: null,
                    currency: null
                };
            }
        };

        product = {
            ...product,
            ...getPriceInfo(),
            images: getImageIds()
        }

        addOrderItemToBasket({
            product,
            quantity,
            isOutletOrder: item.isOutletOrder,
            externalReference: item.externalReference,
            unitPrice: product.price ? parseFloat(product.price.current).toFixed(2) : null,
            price: product.price ? parseFloat(product.price.current * quantity).toFixed(2) : null,
            currency: product.price ? product.price.currency : null,
            moreInfo: item.moreInfo,
            resetData: () => {
                setIsModalOpen(false);
            } 
        })
    };

    const handleCloseImageModal = () => {
        setIsImageModalOpen(false);
    }

    const handleImageModal = (index) => {
        setActiveMediaItemIndex(index);
        setIsImageModalOpen(true);
    }

    const handleCloseModal = () => {
        setIsModalOpen(false);
    }

    const handleModal = (item) => {
        setIsModalOpen(true);
        setProductVariant(item);
        setCanActiveModal(false);
    }

    useEffect(() => {
        if (isSidebarOpen) {
            setCanActiveModal(true);
        }

        if (!canActiveModal && !isModalOpen) {
            setIsSidebarOpen(true)
        }
    }, [canActiveModal, isModalOpen, isSidebarOpen]);

    const handleShowMoreImages = () => {
        if (showMoreImages) {
            setMediaItems(props.mediaItems.slice(0, 6));
        }
        else {
            setMediaItems(props.mediaItems);
        }

        setShowMoreImages(!showMoreImages);
    }

    useEffect(() => {
        if (props.description) {
            const sanitized = GlobalHelper.sanitizeHtml(props.description);
            const clean = marked.parse(sanitized)
            setCleanDescription(clean);
            setPlainText(GlobalHelper.extractTextOnly(clean))
        }
    }, [props.description])

    return (
        <section className="product-detail section">
            <div className="product-detail__container">
                <div className="product-detail__head columns is-desktop">
                    <div className="product-detail__gallery-column">
                        <div className="product-detail__desktop-gallery">
                            <div className="is-flex is-flex-wrap product-detail__product-gallery">
                                {props.mediaItems && props.mediaItems.length > 1 ? (
                                    mediaItems.map((mediaItem, index) => (
                                        <div key={index} className="product-detail__desktop-gallery__desktop-product-image" onClick={() => handleImageModal(index)}>
                                            <LazyLoad offset={LazyLoadConstants.defaultOffset()}>
                                                {mediaItem.mimeType.startsWith("image") ? (
                                                    <ResponsiveImage sources={mediaItem.sources} imageSrc={mediaItem.mediaSrc} imageAlt={mediaItem.mediaAlt} imageTitle={props.title} />
                                                ) : (
                                                    <video autoPlay loop muted playsInline preload='auto'>
                                                        <source src={mediaItem.mediaSrc} type={mediaItem.mimeType} />
                                                    </video>
                                                )}
                                            </LazyLoad>
                                        </div>
                                    ))
                                ) : (
                                    props.mediaItems.length == 1 && (
                                        <div className="product-detail__desktop-gallery__desktop-product-image__single" onClick={() => handleImageModal(0)}>
                                            <LazyLoad offset={LazyLoadConstants.defaultOffset()}>
                                                {props.mediaItems[0].mimeType.startsWith("image") ? (
                                                    <ResponsiveImage sources={props.mediaItems[0].sources} imageSrc={props.mediaItems[0].mediaSrc} imageAlt={props.mediaItems[0].mediaAlt} imageTitle={props.title} />
                                                ) : (
                                                    <video autoPlay loop muted playsInline preload='auto'>
                                                        <source src={props.mediaItems[0].mediaSrc} type={props.mediaItems[0].mimeType} />
                                                    </video>
                                                )}
                                            </LazyLoad>
                                        </div>
                                    )
                                )}

                            </div>
                            {props.mediaItems && props.mediaItems.length > 6 &&
                                <div className="product-detail__more-product-images">
                                    {showMoreImages ? (
                                        <div className="is-flex is-justify-content-center">
                                            <Button
                                                variant="outlined"
                                                onClick={handleShowMoreImages}
                                                color="inherit">
                                                {props.seeLessText}
                                            </Button>
                                        </div>
                                    ) : (
                                        <div className="is-flex is-justify-content-center">
                                            <Button
                                                variant="outlined"
                                                onClick={handleShowMoreImages}
                                                color="inherit">
                                                {props.seeMoreText}
                                            </Button>
                                        </div>
                                    )}
                                </div>
                            }
                        </div>
                        <div className="product-detail__mobile-gallery">
                            {props.mediaItems && props.mediaItems.length > 0 &&
                                <Splide
                                    options={{
                                        type: "slide",
                                        pagination: false
                                    }}
                                >
                                    {props.mediaItems.map((mediaItem, index) => {
                                        return (
                                            <SplideSlide key={index}>
                                                <LazyLoad offset={LazyLoadConstants.defaultOffset()} className="product-detail__mobile-gallery__mobile-product-image">
                                                    {mediaItem.mimeType.startsWith("image") ? (
                                                        <ResponsiveImage sources={mediaItem.sources} imageSrc={mediaItem.mediaSrc} imageAlt={mediaItem.mediaAlt} imageTitle={props.title} />
                                                    ) : (
                                                        <video autoPlay loop muted playsInline preload='auto'>
                                                            <source src={mediaItem.mediaSrc} type={mediaItem.mimeType} />
                                                        </video>
                                                    )}
                                                </LazyLoad>
                                            </SplideSlide>
                                        )
                                    })
                                    }
                                </Splide>
                            }
                        </div>
                    </div>
                    <div className="product-detail__description-column">
                        <p className="product-detail__sku">
                            {props.skuLabel} {props.sku}
                            <CopyButton
                                copiedText={props.copiedText}
                                copyTextError={props.copyTextError}
                                copyToClipboardText={props.copyToClipboardText}
                                text={props.sku}
                                label={props.skuLabel}
                            />
                        </p>
                        {props.ean &&
                            <p className="product-detail__ean">
                                {props.eanLabel} {props.ean}
                                <CopyButton
                                    copiedText={props.copiedText}
                                    copyTextError={props.copyTextError}
                                    copyToClipboardText={props.copyToClipboardText}
                                    text={props.ean}
                                    label={props.eanLabel}
                                />
                            </p>
                        }
                        <h1 className="title is-4 mt-1">
                            {props.title} 
                            <CopyButton
                                copiedText={props.copiedText}
                                copyTextError={props.copyTextError}
                                copyToClipboardText={props.copyToClipboardText}
                                text={props.title}
                                label={props.title}
                            />
                        </h1>
                        <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                        {props.outletTitle && !props.price &&
                            <div className="product-details__discount">{props.outletTitleLabel} {props.outletTitle}</div>
                        }
                        {props.price &&
                            <Price 
                                {...props.price}
                            />
                        }
                        <div className="product-detail__availability mt-3">
                            {props.inStock &&
                                <Availability
                                    label={props.inStockLabel}
                                    availableQuantity={props.availableQuantity}
                                />
                            }
                            {props.inOutlet && 
                                <div className="is-flex">
                                    <Availability
                                        label={props.inOutletLabel}
                                        availableQuantity={props.availableOutletQuantity}
                                    />
                                    {props.outletTitle && 
                                        <div className="product-details__discount ml-2">({props.outletTitleLabel} {props.outletTitle})</div>
                                    }
                                </div>
                            }
                        </div>
                        {props.isAuthenticated &&
                            <div className="product-detail__add-to-cart-button">
                                {props.isProductVariant ? (
                                    <div className="row">
                                        <Button type="text" variant="contained" color="primary" onClick={() => setIsModalOpen(true)}>
                                            {props.basketLabel}
                                        </Button>
                                    </div>
                                ) : (
                                    <div className="product-detail__add-to-cart-button">
                                        <Button type="text" variant="contained" color="primary" onClick={() => setIsSidebarOpen(true)}>
                                            {props.basketLabel}
                                        </Button>
                                    </div>
                                )}
                            </div>
                        }
                        {props.description && 
                            <div className="product-detail__product-description">
                                <h3 className="product-detail__feature-title">
                                    {props.descriptionLabel}
                                    <CopyButton
                                       copiedText={props.copiedText}
                                        copyTextError={props.copyTextError}
                                        copyToClipboardText={props.copyToClipboardText}
                                        text={plainText}
                                        label={props.descriptionLabel}
                                    />
                                </h3>
                                <div dangerouslySetInnerHTML={{ __html: cleanDescription }}></div>
                            </div>
                        }
                        {props.features && props.features.length > 0 &&
                            <div className="mt-2">
                                {showMore ? (
                                    <Fragment>
                                        <div className="is-flex is-justify-content-center">
                                            <span className="is-flex is-align-content-center is-text button" onClick={() => setShowMore(false)}>{props.readLessText} <ExpandLess /></span>
                                        </div>
                                        <div className="product-detail__product-information">
                                            <h3 className="product-detail__feature-title">{props.productInformationLabel}</h3>
                                            <div className="product-detail__product-information-list">
                                                <dl>
                                                    {props.features.map((item, index) =>
                                                        item.value && item.value.trim() !== '' && (
                                                            <Fragment key={item.key}>
                                                                <dt>{item.key}</dt>
                                                                <dd>{item.value}</dd>
                                                            </Fragment>
                                                        ))}
                                                </dl>
                                            </div>
                                        </div>
                                    </Fragment>
                                ) : (
                                    <div className="is-flex is-justify-content-center">
                                        <span className="button is-flex is-align-content-center is-text" onClick={() => setShowMore(true)}>{props.readMoreText} <ExpandMore /></span>
                                    </div>
                                )}
                            </div>
                        }
                    </div>
                </div>
                <Sidebar
                    productId={props.productId}
                    isOpen={isSidebarOpen}
                    manyUses={false}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleModal}
                    labels={props.sidebar}
                />
                <CarouselGrid items={props.productVariants} className="pt-6" />
                {props.files &&
                    <Files {...props.files} />
                }
                <Modal
                    isOpen={isModalOpen}
                    outletOrder={false}
                    setIsOpen={setIsModalOpen}
                    handleClose={handleCloseModal}
                    maxOutletValue={
                        productVariant 
                            ? QuantityCalculatorService.calculateMaxQuantity(
                                orderItems, 'outletQuantity', productVariant.availableOutletQuantity, productVariant.subtitle
                              ) 
                            : QuantityCalculatorService.calculateMaxQuantity(
                                orderItems, 'outletQuantity', props.availableOutletQuantity, props.sku
                            )
                        }
                    outletQuantityInBasket={
                        productVariant 
                            ? QuantityCalculatorService.getCurrentQuantity(
                                orderItems, 'outletQuantity', productVariant.subtitle
                              ) 
                            : QuantityCalculatorService.getCurrentQuantity(
                                orderItems, 'outletQuantity', props.sku
                            )
                        }
                    handleOrder={handleAddOrderItemClick}
                    product={productVariant ? productVariant : props}
                    labels={props.modal}
                />
                <ProductDetailModal
                    isOpen={isImageModalOpen}
                    handleClose={handleCloseImageModal}
                    mediaItems={props.mediaItems}
                    title={props.title}
                    index={activeMediaItemIndex}
                />
            </div>
        </section>
    );
}

ProductDetail.propTypes = {
    title: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    sku: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    brandUrl: PropTypes.string.isRequired,
    brandName: PropTypes.string.isRequired,
    pricesLabel: PropTypes.string.isRequired,
    productInformationLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string,
    inStock: PropTypes.bool.isRequired,
    availableQuantity: PropTypes.number,
    restockableInDaysLabel: PropTypes.string,
    restockableInDays: PropTypes.number,
    descriptionLabel: PropTypes.string.isRequired,
    productDescription: PropTypes.string,
    productVariants: PropTypes.array,
    isProductVariant: PropTypes.bool,
    isAuthenticated: PropTypes.bool,
    images: PropTypes.array,
    files: PropTypes.object,
    sidebar: PropTypes.object,
    modal: PropTypes.object,
    addedProduct: PropTypes.string,
    eanLabel: PropTypes.string.isRequired,
    readLessText: PropTypes.string.isRequired,
    readMoreText: PropTypes.string.isRequired,
    maxAllowedOrderQuantity: PropTypes.number,
    maxAllowedOrderQuantityErrorMessage: PropTypes.string,
    copiedText: PropTypes.string.isRequired,
    copyToClipboardText: PropTypes.string.isRequired,
    copyTextError: PropTypes.string.isRequired
};

export default ProductDetail;
