import React, { Fragment, useContext, useEffect, useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button } from "@mui/material";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import { Context } from "../../../../shared/stores/Store";
import Sidebar from "../../../../shared/components/Sidebar/Sidebar";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import Modal from "../../../../shared/components/Modal/Modal";
import { ExpandMore, ExpandLess } from "@mui/icons-material"
import { marked } from "marked";

import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";
import {Swiper, SwiperSlide} from 'swiper/react';
import { Scrollbar, Navigation } from 'swiper';


function ProductDetail(props) {
    const [state, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = useState(props.basketId ? props.basketId : null);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null);
    const [canActiveModal, setCanActiveModal] = useState(true);
    const [showMore, setShowMore] = useState(false);

    const [fabricsSliderOnStart, setFabricsSliderOnStart] = useState(true);
    const [fabricsSliderOnEnd, setFabricsSliderOnEnd] = useState(false);

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        let product = props;
        if (productVariant){
            product = {
                productId: productVariant.id, 
                sku: productVariant.subtitle, 
                title: productVariant.title,
                images: productVariant.images,
            }
        }

        const quantity = parseInt(item.quantity);
        const stockQuantity = parseInt(item.stockQuantity);
        const outletQuantity = parseInt(item.outletQuantity);

        const totalQuantity = quantity + stockQuantity + outletQuantity;
        const orderItem = {
            productId: product.productId,
            sku: product.sku,
            name: product.title,
            imageId: product.images ? product.images[0].id : null,
            quantity: quantity,
            stockQuantity: stockQuantity,
            outletQuantity: outletQuantity,
            externalReference: item.externalReference, 
            deliveryFrom:  moment(item.deliveryFrom).startOf("day"), 
            deliveryTo: moment(item.deliveryTo).startOf("day"), 
            moreInfo: item.moreInfo
        }

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        if (totalQuantity <= 0){
            return toast.error(props.quantityErrorMessage);
        }

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    dispatch({ type: "SET_TOTAL_BASKET", payload: parseInt(totalQuantity + state.totalBasketItems) })
                    
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                            setIsModalOpen(false);
                        }
                        else {
                            setOrderItems([]);
                        }
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
    }

    const handleModal = (item) => {
        setIsModalOpen(true);
        setProductVariant(item);
        setCanActiveModal(false);
    }

    useEffect(() => {
        if (isSidebarOpen){
            setCanActiveModal(true);
        }

        if (!canActiveModal && !isModalOpen){
            setIsSidebarOpen(true)
        }
    }, [canActiveModal, isModalOpen, isSidebarOpen]);    

    return (
        <section className="product-detail section">
            <div className="product-detail__head columns is-desktop">
                <div className="product-detail__gallery-column">
                    <div className="desktop-product-gallery">
                        <div className="is-flex is-flex-wrap product-detail__product-gallery">
                            {props.images.map((image, index) => {
                                return (
                                    <div className="product-detail__gallery-column__desktop-product-image" key={index}>
                                        <ResponsiveImage sources={image.sources} imageSrc={image.Src} imageAlt={image.Alt}/>
                                    </div>
                                )
                            })}    
                        </div>                                                
                    </div>                    
                    <div className="mobile-product-gallery">
                            <Swiper
                                modules={[Scrollbar, Navigation]}
                                slidesPerView={1}
                                spaceBetween={16}
                                loop
                                scrollbar
                                navigation
                            >
                                {props.images.map((image, index) => {
                                    return (
                                        <SwiperSlide key={index}>
                                            <div className="product-detail__gallery-column__mobile-product-image">
                                                <ResponsiveImage sources={image.sources} imageSrc={image.Src} imageAlt={image.Alt}/>
                                            </div>
                                        </SwiperSlide>
                                    )                                    
                                })}
                            </Swiper>
                        </div>
                </div>
                <div className="product-detail__description-column">
                    <p className="product-detail__sku">{props.skuLabel}{props.sku}</p>                       
                    {props.ean &&
                        <p className="product-detail__ean">{props.eanLabel} {props.ean}</p>
                    }
                    <h1 className="title is-4">{props.title}</h1>  
                    <h2 className="subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                    {props.outletTitle &&
                        <div className="product-details__discount">{props.outletTitleLabel} {props.outletTitle}</div>     
                    }
                    {props.inStock && props.availableQuantity && props.availableQuantity > 0 &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel} {props.availableQuantity}
                             {props.expectedDelivery && 
                               <div className="product-detail__expected-delivery">{props.expectedDeliveryLabel} {moment.utc(props.expectedDelivery).local().format("L")}</div>
                            }
                        </div>
                    }
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
                             <h3 className="product-detail__feature-title">{props.descriptionLabel}</h3>
                             <div dangerouslySetInnerHTML={{__html: marked.parse(props.description)}}></div>
                         </div>
                     }
                </div>                
            </div>
        </section>
        // <section className="product-detail section">
        //     <div className="product-detail__head columns is-tablet">
        //         <div className="column is-6"> // Kolumna galeria
        //             {props.images && props.images.length &&
        //                 <div className="product-detail__image-gallery">
        //                     <ImageGallery items={props.images} />
        //                 </div>
        //             }
        //         </div>
        //         <div className="column is-4"> // kolumna opis
        //             <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
        //             {props.ean &&
        //                 <p className="product-detail__ean">{props.eanLabel} {props.ean}</p>
        //             }
        //             <h1 className="title is-4 mt-1">{props.title}</h1>
        //             <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
        //             {props.outletTitle &&
        //                 <div className="product-details__discount">{props.outletTitleLabel} {props.outletTitle}</div>     
        //             }
        //             {props.inStock && props.availableQuantity && props.availableQuantity > 0 &&
        //                 <div className="product-detail__in-stock">
        //                     {props.inStockLabel} {props.availableQuantity}
        //                     {props.expectedDelivery && 
        //                         <div className="product-detail__expected-delivery">{props.expectedDeliveryLabel} {moment.utc(props.expectedDelivery).local().format("L")}</div>
        //                     }
        //                 </div>
        //             }
        //             {props.inOutlet && props.availableOutletQuantity && props.availableOutletQuantity > 0 &&
        //                 <div className="product-detail__in-stock">
        //                     {props.inOutletLabel} {props.availableOutletQuantity}
        //                 </div>
        //             }
        //             {props.isAuthenticated && 
        //                 <div className="product-detail__add-to-cart-button">
        //                     {props.isProductVariant ? (
        //                         <div className="row">
        //                             <Button type="text" variant="contained" color="primary" onClick={() => setIsModalOpen(true)}>
        //                                 {props.basketLabel}
        //                             </Button>
        //                         </div>
        //                     ) : (
        //                         <div className="product-detail__add-to-cart-button">
        //                             <Button type="text" variant="contained" color="primary" onClick={() => setIsSidebarOpen(true)}>
        //                                 {props.basketLabel}
        //                             </Button>
        //                         </div>
        //                     )}
        //                 </div>
        //             }
        //             {props.description &&
        //                 <div className="product-detail__product-description">
        //                     <h3 className="product-detail__feature-title">{props.descriptionLabel}</h3>
        //                     <div dangerouslySetInnerHTML={{__html: marked.parse(props.description)}}></div>
        //                 </div>
        //             }
        //             {props.features && props.features.length > 0 &&
        //                 <div className="mt-2">
        //                     {showMore ? (
        //                         <Fragment>
        //                             <div className="is-flex is-justify-content-center">
        //                                 <span className="is-flex is-align-content-center is-text button" onClick={() => setShowMore(false)}>{props.readLessText} <ExpandLess/></span>
        //                             </div>
        //                             <div className="product-detail__product-information">
        //                                 <h3 className="product-detail__feature-title">{props.productInformationLabel}</h3>
        //                                 <div className="product-detail__product-information-list">
        //                                     <dl>
        //                                         {props.features.map((item, index) =>
        //                                             <Fragment key={item.key}>
        //                                                 <dt>{item.key}</dt>
        //                                                 <dd>{item.value}</dd>
        //                                             </Fragment>
        //                                         )}
        //                                     </dl>
        //                                 </div>
        //                             </div>
        //                         </Fragment>
        //                     ) : (
        //                         <div className="is-flex is-justify-content-center">
        //                             <span className="button is-flex is-align-content-center is-text" onClick={() => setShowMore(true)}>{props.readMoreText} <ExpandMore/></span>
        //                         </div>
        //                     )}
        //                 </div>
        //             }
        //         </div>
        //         <Sidebar 
        //             productId={props.productId}
        //             isOpen={isSidebarOpen}
        //             manyUses={false}
        //             setIsOpen={setIsSidebarOpen}
        //             handleOrder={handleModal}
        //             labels={props.sidebar}
        //         />
        //     </div>
        //     <CarouselGrid items={props.productVariants} className="pt-6"/>
        //     {props.files &&
        //         <Files {...props.files} />
        //     }
        //     <Modal
        //         isOpen={isModalOpen}
        //         setIsOpen={setIsModalOpen}
        //         handleClose={handleCloseModal}
        //         maxOutletValue={productVariant ? productVariant.availableOutletQuantity : props.availableOutletQuantity}
        //         maxStockValue={productVariant ? productVariant.availableQuantity : props.availableQuantity}
        //         handleOrder={handleAddOrderItemClick}
        //         product={productVariant ? productVariant : props}
        //         labels={props.modal}
        //     />
        // </section>        
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
    readMoreText: PropTypes.string.isRequired
};

export default ProductDetail;